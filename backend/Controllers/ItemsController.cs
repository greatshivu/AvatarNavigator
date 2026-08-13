using Microsoft.AspNetCore.Mvc;
using AvatarNavigator.API.Models;
using AvatarNavigator.API.Services;

namespace AvatarNavigator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(IItemService itemService, ILogger<ItemsController> logger)
        {
            _itemService = itemService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetAllItems()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItemById(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Item>>> SearchItems([FromQuery] string term)
        {
            var items = await _itemService.SearchItemsAsync(term);
            return Ok(items);
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsByCategory(string category)
        {
            var items = await _itemService.GetItemsByCategoryAsync(category);
            return Ok(items);
        }

        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<Item>>> FilterItems([FromBody] FilterCriteria criteria)
        {
            var items = await _itemService.FilterItemsAsync(criteria);
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<Item>> CreateItem([FromBody] Item item)
        {
            item.CreatedAt = DateTime.UtcNow;
            item.UpdatedAt = DateTime.UtcNow;
            var createdItem = await _itemService.CreateItemAsync(item);
            return CreatedAtAction(nameof(GetItemById), new { id = createdItem.Id }, createdItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] Item item)
        {
            if (id != item.Id)
                return BadRequest();

            var existingItem = await _itemService.GetItemByIdAsync(id);
            if (existingItem == null)
                return NotFound();

            await _itemService.UpdateItemAsync(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
                return NotFound();

            await _itemService.DeleteItemAsync(id);
            return NoContent();
        }
    }
}
