using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100)]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100)]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "OIB is required.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "OIB must contain exactly 11 digits.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "OIB can contain only numbers.")]
        public string OIB { get; set; } = string.Empty;

        [Required(ErrorMessage = "JMBG is required.")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "JMBG must contain exactly 13 digits.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "JMBG can contain only numbers.")]
        public string JMBG { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm the password.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}