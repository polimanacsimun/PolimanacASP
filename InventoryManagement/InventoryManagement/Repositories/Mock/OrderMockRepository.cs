using System;
using System.Collections.Generic;
using System.Linq;
using InventoryManagement.Domain.Models;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Repositories.Mock
{
    public class OrderMockRepository
    {
        private readonly List<Order> _orders;

        public OrderMockRepository()
        {
            // Create mock users for order assignment
            var toniPeric = new User
            {
                Id = 1,
                FirstName = "Toni",
                LastName = "Perić",
                Email = "toni.peric@inventory.com",
                Role = UserRole.Employee,
                RegistrationDate = new DateTime(2023, 6, 15),
                IsActive = true
            };

            var majaBabic = new User
            {
                Id = 2,
                FirstName = "Maja",
                LastName = "Babić",
                Email = "maja.babic@inventory.com",
                Role = UserRole.Administrator,
                RegistrationDate = new DateTime(2020, 3, 20),
                IsActive = true
            };

            var ivanRidic = new User
            {
                Id = 3,
                FirstName = "Ivan",
                LastName = "Riđić",
                Email = "ivan.ridic@inventory.com",
                Role = UserRole.Customer,
                RegistrationDate = new DateTime(2024, 1, 10),
                IsActive = true
            };

            // Create mock products for order items
            var businessLaptop = new Product
            {
                Id = 1,
                Name = "Business Laptop",
                Description = "15-inch laptop with i7 processor and 16GB RAM",
                Price = 950m,
                UnitOfMeasure = "piece",
                MinimumStock = 5,
                CreatedAt = new DateTime(2025, 1, 10),
                IsActive = true,
                Type = ProductType.Physical
            };

            var printerPaper = new Product
            {
                Id = 2,
                Name = "Printer Paper A4",
                Description = "500-sheet ream of A4 paper",
                Price = 14.5m,
                UnitOfMeasure = "pack",
                MinimumStock = 20,
                CreatedAt = new DateTime(2025, 1, 15),
                IsActive = true,
                Type = ProductType.Physical
            };

            var cloudLicense = new Product
            {
                Id = 3,
                Name = "Cloud Storage License",
                Description = "1-year subscription for 1TB storage",
                Price = 79m,
                UnitOfMeasure = "license",
                MinimumStock = 0,
                CreatedAt = new DateTime(2025, 2, 5),
                IsActive = true,
                Type = ProductType.Digital
            };

            // Create Order 1: Toni - ORD-2026-001
            var order1 = new Order
            {
                Id = 1,
                OrderNumber = "ORD-2026-001",
                OrderDate = new DateTime(2026, 3, 15),
                TotalPrice = 2850.00m,
                Status = OrderStatus.Processing,
                DeliveryDate = new DateTime(2026, 3, 20),
                Note = "Urgent equipment delivery for office setup",
                User = toniPeric,
                OrderItems = new List<OrderItem>()
            };

            order1.OrderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    Id = 1,
                    Quantity = 3,
                    UnitPrice = 950m,
                    TotalPrice = 2850m,
                    Discount = 0m,
                    CreatedAt = new DateTime(2026, 3, 15, 10, 30, 0),
                    Order = order1,
                    Product = businessLaptop
                }
            };

            // Create Order 2: Maja - ORD-2026-002
            var order2 = new Order
            {
                Id = 2,
                OrderNumber = "ORD-2026-002",
                OrderDate = new DateTime(2026, 2, 28),
                TotalPrice = 145.00m,
                Status = OrderStatus.Delivered,
                DeliveryDate = new DateTime(2026, 3, 5),
                Note = "Regular office supplies replenishment",
                User = majaBabic,
                OrderItems = new List<OrderItem>()
            };

            order2.OrderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    Id = 2,
                    Quantity = 10,
                    UnitPrice = 14.5m,
                    TotalPrice = 145m,
                    Discount = 0m,
                    CreatedAt = new DateTime(2026, 2, 28, 14, 15, 0),
                    Order = order2,
                    Product = printerPaper
                }
            };

            // Create Order 3: Ivan - ORD-2026-003
            var order3 = new Order
            {
                Id = 3,
                OrderNumber = "ORD-2026-003",
                OrderDate = new DateTime(2026, 4, 5),
                TotalPrice = 948.00m,
                Status = OrderStatus.Pending,
                DeliveryDate = null,
                Note = "Cloud storage licenses for team collaboration",
                User = ivanRidic,
                OrderItems = new List<OrderItem>()
            };

            order3.OrderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    Id = 3,
                    Quantity = 12,
                    UnitPrice = 79m,
                    TotalPrice = 948m,
                    Discount = 0m,
                    CreatedAt = new DateTime(2026, 4, 5, 09, 45, 0),
                    Order = order3,
                    Product = cloudLicense
                }
            };

            _orders = new List<Order>
            {
                order1,
                order2,
                order3
            };
        }

        public List<Order> GetAll()
        {
            return _orders.ToList();
        }

        public Order? GetById(int id)
        {
            return _orders.FirstOrDefault(o => o.Id == id);
        }
    }
}
