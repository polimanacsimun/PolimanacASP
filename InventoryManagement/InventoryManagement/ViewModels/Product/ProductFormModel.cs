using System;
using System.ComponentModel.DataAnnotations;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.ViewModels.Product
{
    public class ProductFormModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 100000, ErrorMessage = "Price must be between 0.01 and 100,000.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Unit of measure is required.")]
        [StringLength(30, ErrorMessage = "Unit of measure cannot exceed 30 characters.")]
        public string UnitOfMeasure { get; set; } = string.Empty;

        [Range(0, 100000, ErrorMessage = "Minimum stock must be between 0 and 100,000.")]
        public int MinimumStock { get; set; }

        [Required(ErrorMessage = "Creation date is required.")]
        public DateTime CreatedAt { get; set; } = DateTime.Today;

        public bool IsActive { get; set; } = true;

        public ProductType Type { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int? CategoryId { get; set; }

        public string? CategoryDisplayName { get; set; }

        [Required(ErrorMessage = "Supplier is required.")]
        public int? SupplierId { get; set; }

        public string? SupplierDisplayName { get; set; }
    }
}
