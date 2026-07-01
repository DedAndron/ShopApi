using Shop.Api.Interfaces;
using ShopDomain.Models;

namespace Shop.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private List<Category> _category = new();
        public CategoryService()
        {
            _category.Add(new Category()
            {
                Id = 1,
                Title = "test1",
                Description = "test description",
                Image = "img",
                createdAt = DateTime.Today,
                updatedAt = DateTime.Now,
                isShow = true
            });
            _category.Add(new Category()
            {
                Id = 2,
                Title = "test2",
                Description = "test description",
                Image = "img",
                createdAt = DateTime.Today,
                updatedAt = DateTime.Now,
                isShow = true
            });
        }
        public void AddCategory(Category category)
        {
            _category.Add(category);
        }
        public List<Category> GetAllCategories()
        {
            return _category;
        }
    }
}
