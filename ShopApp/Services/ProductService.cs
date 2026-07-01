using Shop.Api.Interfaces;
using ShopDomain.Models;

namespace Shop.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products = new()
        {
            new Product
            {
                Id = 1,
                Name = "Milk",
                Description = "Fresh milk",
                Price = 40.9m,
                StockQty = 10,
                IsActive = true,
                CategoryId = 1
            },
            new Product
            {
                Id = 2,
                Name = "Bread",
                Description = "White bread",
                Price = 30.5m,
                StockQty = 20,
                IsActive = true,
                CategoryId = 1
            }
        };

        public void AddProduct(Product product)
        {
            if (product.Id == 0)
            {
                product.Id = _products.Count == 0 ? 1 : _products.Max(p => p.Id) + 1;
            }

            _products.Add(product);
        }

        public List<Product> GetAllProducts()
        {
            return _products;
        }
    }
}