using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.WebApi.Models.Auth
{
    public class RegisterRequest
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

        [Required(ErrorMessage = "First name is required.")]
        [MinLength(3, ErrorMessage = "First name must be at least 3 characters long.")]
        public string FirstName { get; set; }

        // ----------------------------------------------------------------------------------------------

        [Required(ErrorMessage = "Last name is required.")]
        [MinLength(1, ErrorMessage = "Last name must be at least 1 character long.")]
        public string LastName { get; set; }

        // ----------------------------------------------------------------------------------------------

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        // ----------------------------------------------------------------------------------------------

        [Required(ErrorMessage = "Birth date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        // ----------------------------------------------------------------------------------------------

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(100, ErrorMessage = "Address must be at most 100 characters long.")]
        public string Address { get; set; }

        // ----------------------------------------------------------------------------------------------
    }
}
