using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Domain.Models
{
    /// <summary>
    /// Junction entity resolving N-N relationship between Order and Product
    /// </summary>
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
        public DateTime CreatedAt { get; set; }

        public int OrderId { get; set; }
        [ForeignKey(nameof(Order))]
        public virtual Order Order { get; set; }

        public int ProductId { get; set; }
        [ForeignKey(nameof(Product))]
        public virtual Product Product { get; set; }
    }
}