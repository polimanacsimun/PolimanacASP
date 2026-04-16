using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string UnitOfMeasure { get; set; }
        public int MinimumStock { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public ProductType Type { get; set; }

        public Category Category { get; set; }
        public Supplier Supplier { get; set; }

        public List<OrderItem> OrderItems { get; set; }
        public List<InventoryItem> InventoryItems { get; set; }

        public Product()
        {
            OrderItems = new List<OrderItem>();
            InventoryItems = new List<InventoryItem>();
        }
    }
}
