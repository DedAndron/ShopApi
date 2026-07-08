using Shop.Application.DTOs.CategoryDTOs;

namespace Shop.Api.Requests.Category
{
    public class CategoryCreateRequest : CategoryCreateDTO
    {
        public IFormFile? Image { get; set; }
    }
}
