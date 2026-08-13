namespace AvatarNavigator.API.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public ItemFilter? Filter { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    public class ItemFilter
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string? Brand { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public int? Rating { get; set; }

        public Item Item { get; set; } = null!;
    }
}
