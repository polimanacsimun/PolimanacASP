using System;
using System.ComponentModel.DataAnnotations;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.ViewModels.Warehouse
{
    public class WarehouseFormModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Warehouse name is required.")]
        [StringLength(100, ErrorMessage = "Warehouse name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Capacity is required.")]
        [Range(1, 1000000, ErrorMessage = "Capacity must be between 1 and 1,000,000.")]
        public int Capacity { get; set; }

        [StringLength(30, ErrorMessage = "Phone number cannot exceed 30 characters.")]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(100, ErrorMessage = "Email address cannot exceed 100 characters.")]
        public string Email { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Manager name cannot exceed 100 characters.")]
        public string Manager { get; set; } = string.Empty;

        [Required(ErrorMessage = "Opening date is required.")]
        public DateTime OpeningDate { get; set; } = DateTime.Today;

        public bool IsActive { get; set; } = true;

        public WarehouseType Type { get; set; } = WarehouseType.Main;
    }
}
