using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.ViewModels.OrderItem
{
    /// <summary>
    /// Form model for OrderItem Create/Edit operations.
    /// Represents the user input for managing line items in orders.
    /// </summary>
    public class OrderItemFormModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the order item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product ordered.
        /// Must be between 1 and 1,000,000 units.
        /// </summary>
        [Range(1, 1000000, ErrorMessage = "Quantity must be between 1 and 1,000,000.")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price of the product at the time of order.
        /// Must be between $0.01 and $1,000,000.
        /// </summary>
        [Range(0.01, 1000000, ErrorMessage = "Unit price must be between 0.01 and 1,000,000.")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the total price for this order item (calculated field).
        /// Typically quantity * unit price.
        /// Must be between $0 and $100,000,000.
        /// </summary>
        [Range(0, 100000000, ErrorMessage = "Total price must be between 0 and 100,000,000.")]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets the discount amount applied to this order item.
        /// Must be between $0 and $1,000,000.
        /// </summary>
        [Range(0, 1000000, ErrorMessage = "Discount must be between 0 and 1,000,000.")]
        public decimal Discount { get; set; }

        /// <summary>
        /// Gets or sets the date and time when this order item was created.
        /// Required field.
        /// </summary>
        [Required(ErrorMessage = "Created date is required.")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the order ID that this item belongs to.
        /// Required for form submission.
        /// </summary>
        [Required(ErrorMessage = "Order is required.")]
        public int? OrderId { get; set; }

        /// <summary>
        /// Gets or sets the display name of the associated order (read-only, for display purposes).
        /// </summary>
        public string? OrderDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the product ID for the item ordered.
        /// Required for form submission.
        /// </summary>
        [Required(ErrorMessage = "Product is required.")]
        public int? ProductId { get; set; }

        /// <summary>
        /// Gets or sets the display name of the selected product (read-only, for display purposes).
        /// </summary>
        public string? ProductDisplayName { get; set; }
    }
}
