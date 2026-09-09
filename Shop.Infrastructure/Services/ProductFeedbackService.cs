using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shop.Application.DTOs.ProductDTOs;
using Shop.Application.Interfaces.Services;
using Shop.Infrastructure.Configuration;
using Shop.Infrastructure.Documents;

namespace Shop.Infrastructure.Services;

public sealed class ProductFeedbackService : IProductFeedbackService
{
    private const string CollectionName = "productFeedback";
    private readonly IMongoCollection<ProductFeedbackDocument> _feedbackCollection;

    public ProductFeedbackService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
    {
        var databaseName = settings.Value.DatabaseName;
        if (string.IsNullOrWhiteSpace(databaseName))
        {
            throw new InvalidOperationException("MongoDB database name is not configured.");
        }

        _feedbackCollection = mongoClient
            .GetDatabase(databaseName)
            .GetCollection<ProductFeedbackDocument>(CollectionName);
    }

    public async Task<string> CreateAsync(
        ProductFeedbackCreateDTO feedback,
        CancellationToken cancellationToken = default)
    {
        var document = new ProductFeedbackDocument
        {
            ProductId = feedback.ProductId,
            Type = feedback.Type,
            Message = feedback.Message,
            CustomerName = feedback.CustomerName,
            CustomerEmail = feedback.CustomerEmail,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _feedbackCollection.InsertOneAsync(document, cancellationToken: cancellationToken);
        return document.Id;
    }
}