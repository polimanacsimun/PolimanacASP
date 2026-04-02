using System;
using System.Collections.Generic;
using System.Linq;
using InventoryManagement.Console.Enums;
using InventoryManagement.Console.Models;

namespace InventoryManagement.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var electronics = new Category
            {
                Id = 1,
                Name = "Electronics",
                Description = "Electronic devices"
            };

            var office = new Category
            {
                Id = 2,
                Name = "Office Supplies",
                Description = "Office materials"
            };

            var consumables = new Category
            {
                Id = 3,
                Name = "Consumables",
                Description = "Consumable inventory items"
            };

            var alphaSupply = new Supplier
            {
                Id = 1,
                Name = "Alpha Supply",
                Address = "123 Main St",
                Phone = "+385 1 222 3333",
                Email = "sales@alpha.com",
                ContactPerson = "Ana Kovač",
                RegistrationDate = new DateTime(2024, 5, 1),
                IsActive = true
            };

            var betaTrade = new Supplier
            {
                Id = 2,
                Name = "Beta Trade",
                Address = "45 Commerce Rd",
                Phone = "+385 1 444 5555",
                Email = "info@beta.com",
                ContactPerson = "Marko Horvat",
                RegistrationDate = new DateTime(2023, 11, 10),
                IsActive = true
            };

            var mainWarehouse = new Warehouse
            {
                Id = 1,
                Name = "Main Warehouse",
                Address = "Industrial Zone 1",
                Capacity = 5000,
                Phone = "+385 1 888 9999",
                Email = "main@warehouse.hr",
                Manager = "Ivana Jurić",
                OpeningDate = new DateTime(2022, 2, 2),
                IsActive = true,
                Type = WarehouseType.Main
            };

            var regionalWarehouse = new Warehouse
            {
                Id = 2,
                Name = "Regional Warehouse",
                Address = "Logistics Park 2",
                Capacity = 2500,
                Phone = "+385 1 777 8888",
                Email = "regional@warehouse.hr",
                Manager = "Dino Novak",
                OpeningDate = new DateTime(2023, 1, 15),
                IsActive = true,
                Type = WarehouseType.Regional
            };

            var laptop = new Product
            {
                Id = 1,
                Name = "Business Laptop",
                Description = "15 inch laptop with i7 and 16GB RAM",
                Price = 950m,
                UnitOfMeasure = "piece",
                MinimumStock = 5,
                CreatedAt = new DateTime(2025, 1, 10),
                IsActive = true,
                Type = ProductType.Physical,
                Category = electronics,
                Supplier = alphaSupply
            };

            var printerPaper = new Product
            {
                Id = 2,
                Name = "Printer Paper A4",
                Description = "500 sheets pack",
                Price = 14.5m,
                UnitOfMeasure = "pack",
                MinimumStock = 20,
                CreatedAt = new DateTime(2025, 1, 15),
                IsActive = true,
                Type = ProductType.Physical,
                Category = office,
                Supplier = betaTrade
            };

            var cloudLicense = new Product
            {
                Id = 3,
                Name = "Cloud Storage License",
                Description = "1 year subscription for 1TB",
                Price = 79m,
                UnitOfMeasure = "license",
                MinimumStock = 0,
                CreatedAt = new DateTime(2025, 2, 5),
                IsActive = true,
                Type = ProductType.Digital,
                Category = consumables,
                Supplier = betaTrade
            };

            electronics.Products.Add(laptop);
            office.Products.Add(printerPaper);
            consumables.Products.Add(cloudLicense);

            alphaSupply.Products.Add(laptop);
            betaTrade.Products.Add(printerPaper);
            betaTrade.Products.Add(cloudLicense);

            var inv1 = new InventoryItem
            {
                Id = 1,
                Product = laptop,
                Warehouse = mainWarehouse,
                QuantityInStock = 18,
                MinimumQuantity = 5,
                MaximumQuantity = 50,
                ShelfLocation = "A1-02",
                LastCheckedAt = new DateTime(2026, 3, 15)
            };

            var inv2 = new InventoryItem
            {
                Id = 2,
                Product = printerPaper,
                Warehouse = mainWarehouse,
                QuantityInStock = 130,
                MinimumQuantity = 20,
                MaximumQuantity = 300,
                ShelfLocation = "B2-08",
                LastCheckedAt = new DateTime(2026, 3, 20)
            };

            var inv3 = new InventoryItem
            {
                Id = 3,
                Product = cloudLicense,
                Warehouse = regionalWarehouse,
                QuantityInStock = 999,
                MinimumQuantity = 0,
                MaximumQuantity = 1000,
                ShelfLocation = "DIG-01",
                LastCheckedAt = new DateTime(2026, 3, 20)
            };

            mainWarehouse.InventoryItems.Add(inv1);
            mainWarehouse.InventoryItems.Add(inv2);
            regionalWarehouse.InventoryItems.Add(inv3);

            laptop.InventoryItems.Add(inv1);
            printerPaper.InventoryItems.Add(inv2);
            cloudLicense.InventoryItems.Add(inv3);

            var user1 = new User
            {
                Id = 1,
                FirstName = "Toni",
                LastName = "Perić",
                Email = "toni.peric@company.hr",
                Role = UserRole.Employee,
                RegistrationDate = new DateTime(2024, 7, 1),
                IsActive = true
            };

            var user2 = new User
            {
                Id = 2,
                FirstName = "Maja",
                LastName = "Babić",
                Email = "maja.babic@company.hr",
                Role = UserRole.Administrator,
                RegistrationDate = new DateTime(2023, 5, 10),
                IsActive = true
            };

            var user3 = new User
            {
                Id = 3,
                FirstName = "Ivan",
                LastName = "Riđić",
                Email = "ivan.ridic@customer.hr",
                Role = UserRole.Customer,
                RegistrationDate = new DateTime(2025, 8, 14),
                IsActive = true
            };

            var order1 = new Order
            {
                Id = 1,
                OrderNumber = "ORD-2026-001",
                OrderDate = new DateTime(2026, 3, 25),
                Status = OrderStatus.Processing,
                DeliveryDate = new DateTime(2026, 3, 29),
                Note = "Urgent delivery for office relocation",
                User = user1
            };

            var orderItem1 = new OrderItem
            {
                Id = 1,
                Order = order1,
                Product = laptop,
                Quantity = 3,
                UnitPrice = 950m,
                Discount = 0m,
                TotalPrice = 3 * 950m,
                CreatedAt = new DateTime(2026, 3, 25)
            };

            order1.OrderItems.Add(orderItem1);
            laptop.OrderItems.Add(orderItem1);

            var order2 = new Order
            {
                Id = 2,
                OrderNumber = "ORD-2026-002",
                OrderDate = new DateTime(2026, 3, 26),
                Status = OrderStatus.Pending,
                DeliveryDate = null,
                Note = "Monthly office supplies",
                User = user2
            };

            var orderItem2 = new OrderItem
            {
                Id = 2,
                Order = order2,
                Product = printerPaper,
                Quantity = 10,
                UnitPrice = 14.5m,
                Discount = 0.5m,
                TotalPrice = 10 * 14.5m - 0.5m,
                CreatedAt = new DateTime(2026, 3, 26)
            };

            order2.OrderItems.Add(orderItem2);
            printerPaper.OrderItems.Add(orderItem2);

            var order3 = new Order
            {
                Id = 3,
                OrderNumber = "ORD-2026-003",
                OrderDate = new DateTime(2026, 3, 27),
                Status = OrderStatus.Delivered,
                DeliveryDate = new DateTime(2026, 3, 28),
                Note = "Cloud licenses batch purchase",
                User = user3
            };

            var orderItem3 = new OrderItem
            {
                Id = 3,
                Order = order3,
                Product = cloudLicense,
                Quantity = 12,
                UnitPrice = 79m,
                Discount = 10m,
                TotalPrice = 12 * 79m - 10m,
                CreatedAt = new DateTime(2026, 3, 27)
            };

            order3.OrderItems.Add(orderItem3);
            cloudLicense.OrderItems.Add(orderItem3);

            user1.Orders.Add(order1);
            user2.Orders.Add(order2);
            user3.Orders.Add(order3);

            var categories = new List<Category> { electronics, office, consumables };
            var suppliers = new List<Supplier> { alphaSupply, betaTrade };
            var products = new List<Product> { laptop, printerPaper, cloudLicense };
            var warehouses = new List<Warehouse> { mainWarehouse, regionalWarehouse };
            var users = new List<User> { user1, user2, user3 };
            var orders = new List<Order> { order1, order2, order3 };
            var inventoryItems = new List<InventoryItem> { inv1, inv2, inv3 };

            order1.TotalPrice = order1.OrderItems.Sum(x => x.TotalPrice);
            order2.TotalPrice = order2.OrderItems.Sum(x => x.TotalPrice);
            order3.TotalPrice = order3.OrderItems.Sum(x => x.TotalPrice);

            System.Console.WriteLine($"Seeded {categories.Count} categories, {products.Count} products, {orders.Count} orders.");
        }
    }
}