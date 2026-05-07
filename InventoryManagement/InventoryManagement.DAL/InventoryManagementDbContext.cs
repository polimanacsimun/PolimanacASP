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

            // Seed data
            
            // Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and equipment" },
                new Category { Id = 2, Name = "Office Supplies", Description = "Office supplies and stationery" },
                new Category { Id = 3, Name = "Consumables", Description = "Consumable items and disposables" }
            );

            // Suppliers
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier
                {
                    Id = 1,
                    Name = "Alpha Supply",
                    Address = "123 Main St, Seattle, WA",
                    Phone = "+1-206-555-1000",
                    Email = "contact@alphasupply.com",
                    ContactPerson = "John Smith",
                    RegistrationDate = new DateTime(2025, 1, 15),
                    IsActive = true
                },
                new Supplier
                {
                    Id = 2,
                    Name = "Beta Trade",
                    Address = "456 Oak Ave, Portland, OR",
                    Phone = "+1-503-555-2000",
                    Email = "sales@betatrade.com",
                    ContactPerson = "Jane Doe",
                    RegistrationDate = new DateTime(2025, 3, 20),
                    IsActive = true
                }
            );

            // Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Business Laptop",
                    Description = "High-performance laptop for business use",
                    Price = 950m,
                    UnitOfMeasure = "unit",
                    MinimumStock = 5,
                    CreatedAt = new DateTime(2026, 1, 10),
                    IsActive = true,
                    Type = InventoryManagement.Domain.Enums.ProductType.Physical,
                    CategoryId = 1,
                    SupplierId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Printer Paper A4",
                    Description = "Standard A4 printer paper, 500 sheets per ream",
                    Price = 14.5m,
                    UnitOfMeasure = "ream",
                    MinimumStock = 50,
                    CreatedAt = new DateTime(2026, 1, 12),
                    IsActive = true,
                    Type = InventoryManagement.Domain.Enums.ProductType.Physical,
                    CategoryId = 2,
                    SupplierId = 2
                },
                new Product
                {
                    Id = 3,
                    Name = "Cloud Storage License",
                    Description = "Annual cloud storage subscription, 100GB capacity",
                    Price = 79m,
                    UnitOfMeasure = "license",
                    MinimumStock = 0,
                    CreatedAt = new DateTime(2026, 1, 15),
                    IsActive = true,
                    Type = InventoryManagement.Domain.Enums.ProductType.Digital,
                    CategoryId = 3,
                    SupplierId = 2
                }
            );

            // Warehouses
            modelBuilder.Entity<Warehouse>().HasData(
                new Warehouse
                {
                    Id = 1,
                    Name = "Main Warehouse",
                    Address = "789 Industrial Blvd, Seattle, WA",
                    Capacity = 50000,
                    Phone = "+1-206-555-3000",
                    Email = "warehouse@company.com",
                    Manager = "Robert Johnson",
                    OpeningDate = new DateTime(2020, 6, 1),
                    IsActive = true,
                    Type = InventoryManagement.Domain.Enums.WarehouseType.Main
                },
                new Warehouse
                {
                    Id = 2,
                    Name = "Regional Warehouse",
                    Address = "321 Logistics Way, Portland, OR",
                    Capacity = 25000,
                    Phone = "+1-503-555-4000",
                    Email = "regional@company.com",
                    Manager = "Sarah Wilson",
                    OpeningDate = new DateTime(2022, 3, 15),
                    IsActive = true,
                    Type = InventoryManagement.Domain.Enums.WarehouseType.Regional
                }
            );

            // Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Toni",
                    LastName = "Perić",
                    Email = "toni.peric@company.com",
                    Role = InventoryManagement.Domain.Enums.UserRole.Employee,
                    RegistrationDate = new DateTime(2025, 6, 1),
                    IsActive = true
                },
                new User
                {
                    Id = 2,
                    FirstName = "Maja",
                    LastName = "Babić",
                    Email = "maja.babic@company.com",
                    Role = InventoryManagement.Domain.Enums.UserRole.Administrator,
                    RegistrationDate = new DateTime(2025, 1, 10),
                    IsActive = true
                },
                new User
                {
                    Id = 3,
                    FirstName = "Ivan",
                    LastName = "Riđić",
                    Email = "ivan.ridic@company.com",
                    Role = InventoryManagement.Domain.Enums.UserRole.Customer,
                    RegistrationDate = new DateTime(2025, 9, 20),
                    IsActive = true
                }
            );

            // Orders
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    OrderNumber = "ORD-2026-001",
                    OrderDate = new DateTime(2026, 4, 1, 10, 30, 0),
                    TotalPrice = 2850m,
                    Status = InventoryManagement.Domain.Enums.OrderStatus.Processing,
                    DeliveryDate = null,
                    Note = "Urgent delivery requested",
                    UserId = 1
                },
                new Order
                {
                    Id = 2,
                    OrderNumber = "ORD-2026-002",
                    OrderDate = new DateTime(2026, 3, 15, 14, 0, 0),
                    TotalPrice = 145m,
                    Status = InventoryManagement.Domain.Enums.OrderStatus.Delivered,
                    DeliveryDate = new DateTime(2026, 3, 18),
                    Note = "Delivered successfully",
                    UserId = 2
                },
                new Order
                {
                    Id = 3,
                    OrderNumber = "ORD-2026-003",
                    OrderDate = new DateTime(2026, 4, 5, 9, 15, 0),
                    TotalPrice = 948m,
                    Status = InventoryManagement.Domain.Enums.OrderStatus.Pending,
                    DeliveryDate = null,
                    Note = "Awaiting payment confirmation",
                    UserId = 3
                }
            );

            // OrderItems
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem
                {
                    Id = 1,
                    Quantity = 3,
                    UnitPrice = 950m,
                    TotalPrice = 2850m,
                    Discount = 0m,
                    CreatedAt = new DateTime(2026, 4, 1, 10, 30, 0),
                    OrderId = 1,
                    ProductId = 1
                },
                new OrderItem
                {
                    Id = 2,
                    Quantity = 10,
                    UnitPrice = 14.5m,
                    TotalPrice = 145m,
                    Discount = 0m,
                    CreatedAt = new DateTime(2026, 3, 15, 14, 0, 0),
                    OrderId = 2,
                    ProductId = 2
                },
                new OrderItem
                {
                    Id = 3,
                    Quantity = 12,
                    UnitPrice = 79m,
                    TotalPrice = 948m,
                    Discount = 0m,
                    CreatedAt = new DateTime(2026, 4, 5, 9, 15, 0),
                    OrderId = 3,
                    ProductId = 3
                }
            );

            // InventoryItems
            modelBuilder.Entity<InventoryItem>().HasData(
                new InventoryItem
                {
                    Id = 1,
                    QuantityInStock = 45,
                    MinimumQuantity = 5,
                    MaximumQuantity = 100,
                    ShelfLocation = "A-01-01",
                    LastCheckedAt = new DateTime(2026, 4, 1),
                    ProductId = 1,
                    WarehouseId = 1
                },
                new InventoryItem
                {
                    Id = 2,
                    QuantityInStock = 320,
                    MinimumQuantity = 50,
                    MaximumQuantity = 500,
                    ShelfLocation = "B-02-03",
                    LastCheckedAt = new DateTime(2026, 4, 2),
                    ProductId = 2,
                    WarehouseId = 1
                },
                new InventoryItem
                {
                    Id = 3,
                    QuantityInStock = 150,
                    MinimumQuantity = 0,
                    MaximumQuantity = 200,
                    ShelfLocation = "C-03-05",
                    LastCheckedAt = new DateTime(2026, 4, 3),
                    ProductId = 3,
                    WarehouseId = 2
                }
            );
        }
    }
}
