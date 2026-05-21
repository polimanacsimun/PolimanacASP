using System;
using System.ComponentModel.DataAnnotations;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.ViewModels.Order
{
    public class OrderFormModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Order number is required.")]
        [StringLength(50, ErrorMessage = "Order number cannot exceed 50 characters.")]
        public string OrderNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Order date is required.")]
        public DateTime OrderDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Total price is required.")]
        [Range(0.01, 1000000, ErrorMessage = "Total price must be between 0.01 and 1,000,000.")]
        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public DateTime? DeliveryDate { get; set; }

        [StringLength(500, ErrorMessage = "Note cannot exceed 500 characters.")]
        public string Note { get; set; } = string.Empty;

        [Required(ErrorMessage = "User is required.")]
        public int? UserId { get; set; }

        public string? UserDisplayName { get; set; }
    }
}
