using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Domain.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string UnitOfMeasure { get; set; }
        public int MinimumStock { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public ProductType Type { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey(nameof(Category))]
        public virtual Category Category { get; set; }

        public int? SupplierId { get; set; }
        [ForeignKey(nameof(Supplier))]
        public virtual Supplier Supplier { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public virtual ICollection<InventoryItem> InventoryItems { get; set; } = new HashSet<InventoryItem>();

        public Product()
        {
            OrderItems = new HashSet<OrderItem>();
            InventoryItems = new HashSet<InventoryItem>();
        }
    }
}
