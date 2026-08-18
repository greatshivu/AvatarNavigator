using AvatarNavigator.API.Models;
using AvatarNavigator.API.Data;
using Microsoft.EntityFrameworkCore;

namespace AvatarNavigator.API.Services
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetAllItemsAsync();
        Task<Item?> GetItemByIdAsync(int id);
        Task<IEnumerable<Item>> SearchItemsAsync(string searchTerm);
        Task<IEnumerable<Item>> GetItemsByCategoryAsync(string category);
        Task<IEnumerable<Item>> FilterItemsAsync(FilterCriteria criteria);
        Task<Item> CreateItemAsync(Item item);
        Task<Item> UpdateItemAsync(Item item);
        Task DeleteItemAsync(int id);
    }

    public class ItemService : IItemService
    {
        private readonly ApplicationDbContext _context;

        public ItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await _context.Items
                .Include(i => i.Filter)
                .ToListAsync();
        }

        public async Task<Item?> GetItemByIdAsync(int id)
        {
            return await _context.Items
                .Include(i => i.Filter)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Item>> SearchItemsAsync(string searchTerm)
        {
            searchTerm = searchTerm.Trim();
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllItemsAsync();
            }

            return await _context.Items
                .Where(i => i.Name.Contains(searchTerm) || i.Description.Contains(searchTerm) || i.Category.Contains(searchTerm))
                .Include(i => i.Filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsByCategoryAsync(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return await GetAllItemsAsync();
            }

            return await _context.Items
                .Where(i => i.Category == category)
                .Include(i => i.Filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Item>> FilterItemsAsync(FilterCriteria criteria)
        {
            var query = _context.Items.Include(i => i.Filter).AsQueryable();

            if (!string.IsNullOrWhiteSpace(criteria.SearchText))
            {
                var term = criteria.SearchText.Trim();
                query = query.Where(i =>
                    i.Name.Contains(term) ||
                    i.Description.Contains(term) ||
                    i.Category.Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(criteria.Category))
                query = query.Where(i => i.Category == criteria.Category);

            if (!string.IsNullOrWhiteSpace(criteria.Brand))
                query = query.Where(i => i.Filter != null && i.Filter.Brand == criteria.Brand);

            if (!string.IsNullOrWhiteSpace(criteria.Color))
                query = query.Where(i => i.Filter != null && i.Filter.Color == criteria.Color);

            if (!string.IsNullOrWhiteSpace(criteria.Size))
                query = query.Where(i => i.Filter != null && i.Filter.Size == criteria.Size);

            if (criteria.InStockOnly == true)
                query = query.Where(i => i.Stock > 0);

            if (criteria.MinPrice.HasValue)
                query = query.Where(i => i.Price >= criteria.MinPrice.Value);

            if (criteria.MaxPrice.HasValue)
                query = query.Where(i => i.Price <= criteria.MaxPrice.Value);

            if (!string.IsNullOrWhiteSpace(criteria.StartDate))
            {
                var startDate = DateTime.Parse(criteria.StartDate);
                query = query.Where(i => i.CreatedAt >= startDate);
            }

            if (!string.IsNullOrWhiteSpace(criteria.EndDate))
            {
                var endDate = DateTime.Parse(criteria.EndDate).Date.AddDays(1).AddTicks(-1);
                query = query.Where(i => i.CreatedAt <= endDate);
            }

            return await query
                .OrderBy(i => i.Id)
                .ToListAsync();
        }

        public async Task<Item> CreateItemAsync(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Item> UpdateItemAsync(Item item)
        {
            item.UpdatedAt = DateTime.UtcNow;
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task DeleteItemAsync(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }

    public class FilterCriteria
    {
        public string? SearchText { get; set; }
        public string? Category { get; set; }
        public string? Brand { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public bool? InStockOnly { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}
