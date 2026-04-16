using System;
using System.Collections.Generic;
using System.Linq;
using InventoryManagement.Domain.Models;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Repositories.Mock
{
    public class UserMockRepository
    {
        private readonly List<User> _users;

        public UserMockRepository()
        {
            var toniPeric = new User
            {
                Id = 1,
                FirstName = "Toni",
                LastName = "Perić",
                Email = "toni.peric@inventory.com",
                Role = UserRole.Employee,
                RegistrationDate = new DateTime(2023, 6, 15),
                IsActive = true,
                Orders = new List<Order>()
            };

            var majaBabic = new User
            {
                Id = 2,
                FirstName = "Maja",
                LastName = "Babić",
                Email = "maja.babic@inventory.com",
                Role = UserRole.Administrator,
                RegistrationDate = new DateTime(2020, 3, 20),
                IsActive = true,
                Orders = new List<Order>()
            };

            var ivanRidic = new User
            {
                Id = 3,
                FirstName = "Ivan",
                LastName = "Riđić",
                Email = "ivan.ridic@inventory.com",
                Role = UserRole.Customer,
                RegistrationDate = new DateTime(2024, 1, 10),
                IsActive = true,
                Orders = new List<Order>()
            };

            // Create mock orders for each user
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

            var order2 = new Order
            {
                Id = 2,
                OrderNumber = "ORD-2026-002",
                OrderDate = new DateTime(2026, 2, 28),
                TotalPrice = 145.00m,
                Status = OrderStatus.Delivered,
                DeliveryDate = new DateTime(2026, 3, 05),
                Note = "Regular office supplies replenishment",
                User = majaBabic,
                OrderItems = new List<OrderItem>()
            };

            var order3 = new Order
            {
                Id = 3,
                OrderNumber = "ORD-2026-003",
                OrderDate = new DateTime(2026, 4, 05),
                TotalPrice = 948.00m,
                Status = OrderStatus.Pending,
                DeliveryDate = null,
                Note = "Cloud storage licenses for team collaboration",
                User = ivanRidic,
                OrderItems = new List<OrderItem>()
            };

            // Assign orders to users
            toniPeric.Orders = new List<Order> { order1 };
            majaBabic.Orders = new List<Order> { order2 };
            ivanRidic.Orders = new List<Order> { order3 };

            _users = new List<User>
            {
                toniPeric,
                majaBabic,
                ivanRidic
            };
        }

        public List<User> GetAll()
        {
            return _users.ToList();
        }

        public User? GetById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }
    }
}
