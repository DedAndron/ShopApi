using Shop.Api.Interfaces;
using ShopDomain.Models;

namespace Shop.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly List<Category> _categories = new()
        {
            new Category
            {
                Id = 1,
                Name = "Dairy",
                Slug = "dairy"
            },
            new Category
            {
                Id = 2,
                Name = "Bakery",
                Slug = "bakery"
            }
        };

        public void AddCategory(Category category)
        {
            if (category.Id == 0)
            {
                category.Id = _categories.Count == 0 ? 1 : _categories.Max(c => c.Id) + 1;
            }

            _categories.Add(category);
        }

        public List<Category> GetAllCategories()
        {
            return _categories;
        }
    }
}