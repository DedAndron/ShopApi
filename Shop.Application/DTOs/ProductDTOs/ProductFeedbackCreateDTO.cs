namespace Shop.Application.DTOs.ProductDTOs;

public sealed class ProductFeedbackCreateDTO
{
    public int ProductId { get; init; }
    public string Type { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string? CustomerName { get; init; }
    public string? CustomerEmail { get; init; }
}