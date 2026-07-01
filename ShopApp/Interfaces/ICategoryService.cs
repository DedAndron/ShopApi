using ShopDomain.Models;

namespace Shop.Api.Interfaces
{
    public interface ICategoryService
    {
        List<Category> GetAllCategories();
        void AddCategory(Category category);
    }
}
