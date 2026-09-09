using System.ComponentModel.DataAnnotations;

namespace Shop.Api.Requests.Product;

public sealed class ProductFeedbackCreateRequest
{
    [Required]
    [MaxLength(20)]
    [RegularExpression("^(feedback|question)$", ErrorMessage = "Type must be either 'feedback' or 'question'.")]
    public string Type { get; init; } = string.Empty;

    [Required]
    [MaxLength(2_000)]
    public string Message { get; init; } = string.Empty;

    [MaxLength(100)]
    public string? CustomerName { get; init; }

    [EmailAddress]
    [MaxLength(254)]
    public string? CustomerEmail { get; init; }
}