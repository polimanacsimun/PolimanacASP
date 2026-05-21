using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.ViewModels.Category
{
    /// <summary>
    /// Form model for Category Create/Edit operations.
    /// Used for data transfer between views and controllers with validation.
    /// </summary>
    public class CategoryFormModel
    {
        /// <summary>
        /// Unique identifier for the category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Category name.
        /// Required field, maximum 100 characters.
        /// </summary>
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Category description.
        /// Optional field, maximum 500 characters.
        /// </summary>
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;
    }
}
