using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Domain.Models
{
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Manager { get; set; }
        public DateTime OpeningDate { get; set; }
        public bool IsActive { get; set; }
        public WarehouseType Type { get; set; }

        public virtual ICollection<InventoryItem> InventoryItems { get; set; } = new HashSet<InventoryItem>();

        public Warehouse()
        {
            InventoryItems = new HashSet<InventoryItem>();
        }
    }
}