using Microsoft.AspNetCore.Mvc;
using ShopDomain.Models;

namespace Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<Item> Items = new()
        {
            new Item { Id = 1, Name = "Test" },
            new Item { Id = 2, Name = "Test1" },
            new Item { Id = 3, Name = "Test2" }
        };

        [HttpGet]
        public IActionResult GetItems()
        {
            return Ok(Items);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetItemById([FromRoute] int id)
        {
            var item = Items.FirstOrDefault(i => i.Id == id);

            if (item is null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpGet("search/{name}")]
        public IActionResult GetItemByName([FromRoute] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest();
            }

            var items = Items
                .Where(item => string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(items);
        }
    }
}