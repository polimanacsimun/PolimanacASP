using System;

namespace InventoryManagement.Domain.Models
{
    public class OrderItem
    {
        //spojna klasa koja rijesava N-N izmedju Order i Product
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
        public DateTime CreatedAt { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}