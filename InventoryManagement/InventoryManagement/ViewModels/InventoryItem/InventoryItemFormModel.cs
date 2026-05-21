using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.ViewModels.InventoryItem
{
    /// <summary>
    /// Form model for InventoryItem Create/Edit operations.
    /// Represents the user input for managing inventory items in warehouses.
    /// </summary>
    public class InventoryItemFormModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the inventory item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the current quantity of the product in stock.
        /// Must be between 0 and 1,000,000 units.
        /// </summary>
        [Range(0, 1000000, ErrorMessage = "Quantity in stock must be between 0 and 1,000,000.")]
        public int QuantityInStock { get; set; }

        /// <summary>
        /// Gets or sets the minimum quantity threshold for reordering.
        /// Must be between 0 and 1,000,000 units.
        /// </summary>
        [Range(0, 1000000, ErrorMessage = "Minimum quantity must be between 0 and 1,000,000.")]
        public int MinimumQuantity { get; set; }

        /// <summary>
        /// Gets or sets the maximum quantity capacity for this inventory item.
        /// Must be between 0 and 1,000,000 units.
        /// </summary>
        [Range(0, 1000000, ErrorMessage = "Maximum quantity must be between 0 and 1,000,000.")]
        public int MaximumQuantity { get; set; }

        /// <summary>
        /// Gets or sets the shelf location or storage designation for this item.
        /// Optional, maximum 50 characters.
        /// </summary>
        [StringLength(50, ErrorMessage = "Shelf location cannot exceed 50 characters.")]
        public string ShelfLocation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when this inventory item was last checked/audited.
        /// Required field.
        /// </summary>
        [Required(ErrorMessage = "Last checked date is required.")]
        public DateTime LastCheckedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the product ID associated with this inventory item.
        /// Required for form submission.
        /// </summary>
        [Required(ErrorMessage = "Product is required.")]
        public int? ProductId { get; set; }

        /// <summary>
        /// Gets or sets the display name of the selected product (read-only, for display purposes).
        /// </summary>
        public string? ProductDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the warehouse ID where this inventory item is stored.
        /// Required for form submission.
        /// </summary>
        [Required(ErrorMessage = "Warehouse is required.")]
        public int? WarehouseId { get; set; }

        /// <summary>
        /// Gets or sets the display name of the selected warehouse (read-only, for display purposes).
        /// </summary>
        public string? WarehouseDisplayName { get; set; }
    }
}
