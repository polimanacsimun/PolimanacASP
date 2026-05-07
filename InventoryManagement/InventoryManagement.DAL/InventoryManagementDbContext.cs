using Microsoft.EntityFrameworkCore;
using InventoryManagement.Domain.Models;

namespace InventoryManagement.DAL
{
    public class InventoryManagementDbContext : DbContext
    {
        public InventoryManagementDbContext(DbContextOptions<InventoryManagementDbContext> options)
            : base(options)
        {
        }

        // DbSet properties for all entities
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product entity
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Configure Order entity
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Configure OrderItem entity
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.TotalPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Discount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Configure InventoryItem unique constraint on ProductId + WarehouseId
            modelBuilder.Entity<InventoryItem>()
                .HasIndex(ii => new { ii.ProductId, ii.WarehouseId })
                .IsUnique();

            // Configure relationships if needed
            // Category 1-N Product (already defined in Product via CategoryId FK)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Supplier 1-N Product (already defined in Product via SupplierId FK)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.SetNull);

            // Warehouse 1-N InventoryItem
            modelBuilder.Entity<InventoryItem>()
                .HasOne(ii => ii.Warehouse)
                .WithMany(w => w.InventoryItems)
                .HasForeignKey(ii => ii.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product 1-N InventoryItem
            modelBuilder.Entity<InventoryItem>()
    .HasOne(ii => ii.Product)
    .WithMany(p => p.InventoryItems)
    .HasForeignKey(ii => ii.ProductId)
    .OnDelete(DeleteBehavior.Restrict);

            // User 1-N Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order 1-N OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product 1-N OrderItem
modelBuilder.Entity<OrderItem>()
    .HasOne(oi => oi.Product)
    .WithMany(p => p.OrderItems)
    .HasForeignKey(oi => oi.ProductId)
    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
