using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShopDomain.Models;

namespace Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private List<Item> _items = new();
        [HttpGet("/api/items")]
        public IActionResult GetItems()
        {
            _items.Add(new Item()
            {
                Id = 1,
                Name = "Test",
            });
            _items.Add(new Item()
            {
                Id = 2,
                Name = "Test1",
            });
            _items.Add(new Item()
            {
                Id = 3,
                Name = "Test2",
            });
            return Ok(_items);
        }
        [HttpGet("/api/items/{id}")]
        public IActionResult GetItemById(int id)
        {
            Item item = new Item()
            {
                Id = id,
                Name = $"Test{id}",
            };
            return Ok(item);
        }
        [HttpGet("/api/items/search/{name}")]
        public IActionResult GetItemByName(string name)
        {
            List<Item> items = new List<Item>();
            if (name == null)
            {
                return BadRequest();
            }
            foreach (var item in items)
            {
                if (item.Name == name)
                {
                    items.Add(item);
                }
            }
            return Ok(items);
        }
    }
}
