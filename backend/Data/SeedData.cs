using AvatarNavigator.API.Models;

namespace AvatarNavigator.API.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            // If data already exists, skip seeding
            if (context.Orders.Any() || context.Items.Any())
            {
                return;
            }

            // Seed 10K Items
            var items = GenerateItems(10000);
            context.Items.AddRange(items);
            await context.SaveChangesAsync();

            // Seed 10K Orders
            var orders = GenerateOrders(10000, items);
            context.Orders.AddRange(orders);
            await context.SaveChangesAsync();
        }

        private static List<Item> GenerateItems(int count)
        {
            var categories = new[] { "Electronics", "Clothing", "Home", "Sports", "Books", "Toys" };
            var brands = new[] { "Brand A", "Brand B", "Brand C", "Brand D", "Brand E" };
            var colors = new[] { "Red", "Blue", "Green", "Black", "White", "Yellow" };
            var sizes = new[] { "S", "M", "L", "XL", "XXL" };

            var items = new List<Item>();
            var random = new Random(42); // Seed for reproducibility

            for (int i = 1; i <= count; i++)
            {
                var item = new Item
                {
                    Name = $"Product {i}",
                    Description = $"High quality product {i} with excellent features",
                    Price = (decimal)(random.NextDouble() * 1000 + 10),
                    Stock = random.Next(0, 1000),
                    Category = categories[random.Next(categories.Length)],
                    ImageUrl = $"https://via.placeholder.com/300?text=Product{i}",
                    CreatedAt = DateTime.UtcNow.AddDays(-random.Next(365)),
                    UpdatedAt = DateTime.UtcNow,
                    Filter = new ItemFilter
                    {
                        Brand = brands[random.Next(brands.Length)],
                        Color = colors[random.Next(colors.Length)],
                        Size = sizes[random.Next(sizes.Length)],
                        Rating = random.Next(1, 6)
                    }
                };
                items.Add(item);
            }

            return items;
        }

        private static List<Order> GenerateOrders(int count, List<Item> items)
        {
            var orders = new List<Order>();
            var random = new Random(42);
            var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            for (int i = 1; i <= count; i++)
            {
                var orderItemsCount = random.Next(1, 6);
                var orderItems = new List<OrderItem>();
                decimal totalAmount = 0;

                for (int j = 0; j < orderItemsCount; j++)
                {
                    var item = items[random.Next(items.Count)];
                    var quantity = random.Next(1, 10);
                    var unitPrice = item.Price;
                    var lineTotal = unitPrice * quantity;
                    totalAmount += lineTotal;

                    orderItems.Add(new OrderItem
                    {
                        ItemId = item.Id,
                        Quantity = quantity,
                        UnitPrice = unitPrice
                    });
                }

                var order = new Order
                {
                    OrderNumber = $"ORD-{i:D8}",
                    OrderDate = DateTime.UtcNow.AddDays(-random.Next(365)),
                    CustomerName = $"Customer {i}",
                    CustomerEmail = $"customer{i}@example.com",
                    TotalAmount = totalAmount,
                    Status = statuses[random.Next(statuses.Length)],
                    ShippingAddress = $"{i} Main Street, City {i % 100}, State, ZIP",
                    CreatedAt = DateTime.UtcNow.AddDays(-random.Next(365)),
                    UpdatedAt = DateTime.UtcNow,
                    OrderItems = orderItems
                };
                orders.Add(order);
            }

            return orders;
        }
    }
}
