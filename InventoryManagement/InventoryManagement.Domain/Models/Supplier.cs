using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Domain.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();

        public Supplier()
        {
            Products = new HashSet<Product>();
        }
    }
}
