using System;
using System.Collections.Generic;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Note { get; set; }

        public User User { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}