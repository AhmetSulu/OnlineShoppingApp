using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.WebApi.Models.Auth
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        // ----------------------------------------------------------------------------------------------

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "Password must be at least 8 and at most 64 characters long.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,64}$",
            ErrorMessage = "Password must contain at least one digit, one uppercase letter, one lowercase letter, and one special character.")]
        public string Password { get; set; }

        // ----------------------------------------------------------------------------------------------

        [Required(ErrorMessage = "Password confirmation is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        // ----------------------------------------------------------------------------------------------
    }
}
