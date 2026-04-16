using System;
using System.Collections.Generic;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }

        public List<Order> Orders { get; set; }

        public User()
        {
            Orders = new List<Order>();
        }
    }
}