using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.WebApi.Models.Auth
{
    public class LoginResponse
    {
        [Required(ErrorMessage = "Message is required.")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Token is required.")]
        [StringLength(256, ErrorMessage = "Token length must be at most 256 characters.")]
        public string Token { get; set; }
    }
}
