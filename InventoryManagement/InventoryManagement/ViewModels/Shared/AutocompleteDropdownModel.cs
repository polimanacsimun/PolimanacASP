namespace InventoryManagement.ViewModels.Shared
{
    /// <summary>
    /// View model for AJAX autocomplete dropdown partial view.
    /// Encapsulates configuration for a reusable autocomplete control that can be used across multiple forms.
    /// </summary>
    public class AutocompleteDropdownModel
    {
        /// <summary>
        /// Label text displayed above the input field.
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Name attribute of the hidden input field that stores the selected ID.
        /// Example: "CategoryId", "SupplierId"
        /// </summary>
        public string HiddenInputName { get; set; } = string.Empty;

        /// <summary>
        /// ID attribute of the hidden input field that stores the selected ID.
        /// Used as the form field name for model binding.
        /// </summary>
        public string HiddenInputId { get; set; } = string.Empty;

        /// <summary>
        /// ID attribute of the display input field (visible to user).
        /// Used by JavaScript to attach autocomplete plugin.
        /// </summary>
        public string DisplayInputId { get; set; } = string.Empty;

        /// <summary>
        /// URL endpoint for AJAX autocomplete search.
        /// Example: "/lookup/categories", "/lookup/suppliers"
        /// </summary>
        public string SearchUrl { get; set; } = string.Empty;

        /// <summary>
        /// Currently selected ID value (from existing entity).
        /// Nullable to support new entity creation scenarios.
        /// </summary>
        public int? SelectedId { get; set; }

        /// <summary>
        /// Display text for the currently selected value.
        /// Shown in the display input field on initial page load.
        /// </summary>
        public string DisplayValue { get; set; } = string.Empty;

        /// <summary>
        /// Placeholder text for the display input field.
        /// Example: "Search for category...", "Select supplier..."
        /// </summary>
        public string Placeholder { get; set; } = string.Empty;

        /// <summary>
        /// Helper text shown below the input field.
        /// Can provide instructions or hint about autocomplete functionality.
        /// </summary>
        public string HelpText { get; set; } = string.Empty;
    }
}
