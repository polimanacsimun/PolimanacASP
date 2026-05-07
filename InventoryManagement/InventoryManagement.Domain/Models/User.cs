using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Domain.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();

        public User()
        {
            Orders = new HashSet<Order>();
        }
    }
}