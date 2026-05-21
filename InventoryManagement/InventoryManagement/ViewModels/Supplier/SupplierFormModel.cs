using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.ViewModels.Supplier
{
    /// <summary>
    /// Form model for Supplier Create/Edit operations.
    /// Used for data transfer between views and controllers with validation.
    /// </summary>
    public class SupplierFormModel
    {
        /// <summary>
        /// Unique identifier for the supplier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Supplier business name.
        /// Required field, maximum 100 characters.
        /// </summary>
        [Required(ErrorMessage = "Supplier name is required.")]
        [StringLength(100, ErrorMessage = "Supplier name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Supplier business address.
        /// Optional field, maximum 200 characters.
        /// </summary>
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Supplier contact phone number.
        /// Optional field, maximum 30 characters.
        /// </summary>
        [StringLength(30, ErrorMessage = "Phone number cannot exceed 30 characters.")]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Supplier business email address.
        /// Required field, must be valid email format, maximum 100 characters.
        /// </summary>
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(100, ErrorMessage = "Email address cannot exceed 100 characters.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Primary contact person at the supplier.
        /// Optional field, maximum 100 characters.
        /// </summary>
        [StringLength(100, ErrorMessage = "Contact person name cannot exceed 100 characters.")]
        public string ContactPerson { get; set; } = string.Empty;

        /// <summary>
        /// Date when the supplier was registered in the system.
        /// Required field.
        /// </summary>
        [Required(ErrorMessage = "Registration date is required.")]
        public DateTime RegistrationDate { get; set; } = DateTime.Today;

        /// <summary>
        /// Whether the supplier is currently active and available for orders.
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
