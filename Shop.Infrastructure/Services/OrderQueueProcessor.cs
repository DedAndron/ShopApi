using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shop.Api.Interfaces;
using Shop.Application.DTOs.OrderDTOs;
using Shop.Application.Interfaces.Services;
using Shop.Infrastructure.Data;
using ShopDomain.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;

namespace Shop.Infrastructure.Services;

public sealed class OrderQueueProcessor(
    ShopDbContext context,
    IEmailService emailService,
    ILogger<OrderQueueProcessor> logger) : IOrderQueueProcessor
{
    public async Task<bool> ProcessAsync(QueuedOrderDTO queuedOrder, CancellationToken cancellationToken)
    {
        var validationResults = new List<ValidationResult>();
        if (!Validator.TryValidateObject(queuedOrder, new ValidationContext(queuedOrder), validationResults, true) ||
            queuedOrder.Details.Any(detail => detail.ProductId <= 0 || detail.Quantity <= 0))
        {
            logger.LogWarning("Discarded invalid order message for {Email}: {Errors}", queuedOrder.Email,
                string.Join("; ", validationResults.Select(result => result.ErrorMessage)));
            return true;
        }

        var requestedItems = queuedOrder.Details
            .GroupBy(detail => detail.ProductId)
            .Select(group => new { ProductId = group.Key, Quantity = group.Sum(detail => detail.Quantity) })
            .ToList();
        var productIds = requestedItems.Select(item => item.ProductId).ToList();
        await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        var products = await context.Products
            .Where(product => productIds.Contains(product.Id))
            .ToDictionaryAsync(product => product.Id, cancellationToken);

        var unavailable = requestedItems
            .Where(item => !products.TryGetValue(item.ProductId, out var product) || !product.IsActive || product.StockQty < item.Quantity)
            .ToList();

        if (unavailable.Count > 0)
        {
            await transaction.RollbackAsync(cancellationToken);
            var waitingBody = new StringBuilder("Ваше замовлення прийнято в очікування.\n\nНаразі недоступні:\n");
            foreach (var item in unavailable)
            {
                var name = products.TryGetValue(item.ProductId, out var product) ? product.Name : $"Товар #{item.ProductId}";
                waitingBody.AppendLine($"- {name}: {item.Quantity} шт.");
            }

            await emailService.SendAsync(queuedOrder.Email, "Замовлення очікує наявності товарів", waitingBody.ToString(), cancellationToken);
            return true;
        }

        var order = new Order
        {
            Details = requestedItems.Select(item =>
            {
                var product = products[item.ProductId];
                product.StockQty -= item.Quantity;
                return new OrderDetail { ProductId = product.Id, Quantity = item.Quantity, Price = product.Price };
            }).ToList()
        };
        context.Orders.Add(order);
        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        var body = BuildConfirmationBody(order, products);
        await emailService.SendAsync(queuedOrder.Email, $"Підтвердження замовлення №{order.Id}", body, cancellationToken);
        logger.LogInformation("Order {OrderId} was created from the Orders queue for {Email}", order.Id, queuedOrder.Email);
        return true;
    }

    private static string BuildConfirmationBody(Order order, IReadOnlyDictionary<int, Product> products)
    {
        var body = new StringBuilder($"Ваше замовлення №{order.Id} сформовано.\n\nТовари:\n");
        decimal total = 0;
        foreach (var detail in order.Details)
        {
            var lineTotal = detail.Price * detail.Quantity;
            total += lineTotal;
            body.AppendLine($"- {products[detail.ProductId].Name}: {detail.Quantity} × {detail.Price:F2} = {lineTotal:F2}");
        }
        body.AppendLine($"\nЗагальна сума: {total:F2}");
        return body.ToString();
    }
}
