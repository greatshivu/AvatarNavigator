using Microsoft.EntityFrameworkCore;
using AvatarNavigator.API.Models;

namespace AvatarNavigator.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemFilter> ItemFilters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Order configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CustomerEmail).IsRequired().HasMaxLength(256);
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.HasMany(e => e.OrderItems).WithOne(oi => oi.Order).HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);
            });

            // OrderItem configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
                entity.HasOne(e => e.Order).WithMany(o => o.OrderItems).HasForeignKey(e => e.OrderId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Item).WithMany(i => i.OrderItems).HasForeignKey(e => e.ItemId).OnDelete(DeleteBehavior.NoAction);
            });

            // Item configuration
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price).HasPrecision(18, 2);
                entity.HasOne(e => e.Filter).WithOne(f => f.Item).HasForeignKey<ItemFilter>(f => f.ItemId).OnDelete(DeleteBehavior.Cascade);
            });

            // ItemFilter configuration
            modelBuilder.Entity<ItemFilter>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Brand).HasMaxLength(100);
                entity.Property(e => e.Color).HasMaxLength(50);
                entity.Property(e => e.Size).HasMaxLength(50);
            });
        }
    }
}
