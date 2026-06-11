using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Domain.Models
{
    public class AppUser : IdentityUser
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "OIB is required.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "OIB must contain exactly 11 digits.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "OIB can contain only numbers.")]
        public string OIB { get; set; } = string.Empty;

        [Required(ErrorMessage = "JMBG is required.")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "JMBG must contain exactly 13 digits.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "JMBG can contain only numbers.")]
        public string JMBG { get; set; } = string.Empty;
    }
}