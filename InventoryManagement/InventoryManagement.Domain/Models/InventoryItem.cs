using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Domain.Models
{
    /// <summary>
    /// Junction entity resolving N-N relationship between Product and Warehouse
    /// </summary>
    public class InventoryItem
    {
        [Key]
        public int Id { get; set; }
        public int QuantityInStock { get; set; }
        public int MinimumQuantity { get; set; }
        public int MaximumQuantity { get; set; }
        public string ShelfLocation { get; set; }
        public DateTime LastCheckedAt { get; set; }

        public int ProductId { get; set; }
        [ForeignKey(nameof(Product))]
        public virtual Product Product { get; set; }

        public int WarehouseId { get; set; }
        [ForeignKey(nameof(Warehouse))]
        public virtual Warehouse Warehouse { get; set; }
    }
}