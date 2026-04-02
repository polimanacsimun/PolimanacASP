using System;

namespace InventoryManagement.Console.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public int QuantityInStock { get; set; }
        public int MinimumQuantity { get; set; }
        public int MaximumQuantity { get; set; }
        public string ShelfLocation { get; set; }
        public DateTime LastCheckedAt { get; set; }

        public Product Product { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}