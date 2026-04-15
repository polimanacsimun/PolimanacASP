using System;
using System.Collections.Generic;
using InventoryManagement.Console.Enums;

namespace InventoryManagement.Console.Models
{
    public class Warehouse
    {
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

        public List<InventoryItem> InventoryItems { get; set; }

        public Warehouse()
        {
            InventoryItems = new List<InventoryItem>();
        }
    }
}