using Shop.Application.DTOs.ProductDTOs;

namespace Shop.Application.Interfaces.Services;

public interface IProductFeedbackService
{
    Task<string> CreateAsync(ProductFeedbackCreateDTO feedback, CancellationToken cancellationToken = default);
}