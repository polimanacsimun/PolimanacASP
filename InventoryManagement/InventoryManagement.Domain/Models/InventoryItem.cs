using System;

namespace InventoryManagement.Domain.Models
{
    public class InventoryItem
    {
        //spojna klasa koja rijesava N-N izmedju Product i Warehouse
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