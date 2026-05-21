namespace InventoryManagement.ViewModels.Shared
{
    /// <summary>
    /// Reusable view model for custom date/time input partial views.
    /// Provides configuration for date and optional time input fields.
    /// </summary>
    public class DateTimeInputModel
    {
        /// <summary>
        /// Gets or sets the label text displayed above the input field.
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the HTML name attribute for the input element (used for form binding).
        /// </summary>
        public string InputName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the HTML id attribute for the input element (used for JavaScript targeting).
        /// </summary>
        public string InputId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current date/time value displayed in the input field.
        /// </summary>
        public DateTime? Value { get; set; }

        /// <summary>
        /// Gets or sets the placeholder text shown when input is empty.
        /// </summary>
        public string Placeholder { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the help text displayed below the input field for user guidance.
        /// </summary>
        public string HelpText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the time component should be included in the input.
        /// If true, renders datetime-local input; if false, renders date input only.
        /// </summary>
        public bool IncludeTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this input field is required.
        /// </summary>
        public bool IsRequired { get; set; }
    }
}
