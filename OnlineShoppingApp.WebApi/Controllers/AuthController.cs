using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Business.Operations.User;
using OnlineShoppingApp.Business.Operations.User.Dtos;
using OnlineShoppingApp.WebApi.Jwt;
using OnlineShoppingApp.WebApi.Models.Auth;

namespace OnlineShoppingApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        // Constructor to inject the user service
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // ----------------------------------------------------------------------------------------------

        // Endpoint for user registration
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create DTO for adding a new user
            var addUserDto = new AddUserDto
            {
                Email = request.Email,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                BirthDate = request.BirthDate,
                Address = request.Address
            };

            // Attempt to add the user
            var result = await _userService.AddUser(addUserDto);
            if (result.IsSuccess)
                return Ok(new { Message = "Registration successful!" });
            else
                return BadRequest(result.Message);
        }

        // ----------------------------------------------------------------------------------------------

        // Endpoint for user login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Attempt to login the user
            var result = _userService.LoginUser(new LoginUserDto { Email = request.Email, Password = request.Password });
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            var user = result.Data;
            var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            // Generate JWT token for the user
            var token = JwtHelper.GenerateJwtToken(new JwtDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.UserType,
                SecretKey = configuration["Jwt:SecretKey"]!,
                Issuer = configuration["Jwt:Issuer"]!,
                Audience = configuration["Jwt:Audience"]!,
                ExpireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]!)
            });

            return Ok(new LoginResponse()
            {
                Message = "Login successful.",
                Token = token
            });
        }

        // ----------------------------------------------------------------------------------------------

        // Endpoint for changing password, accessible only to Admin users
        [Authorize(Roles = "Admin")]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create DTO for changing the password
            var changePasswordDto = new ChangePasswordDto
            {
                Email = request.Email,
                OldPassword = request.OldPassword,
                NewPassword = request.NewPassword,
                ConfirmNewPassword = request.ConfirmNewPassword
            };

            // Attempt to change the password
            var result = await _userService.ChangePassword(changePasswordDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(new { Message = "Password changed successfully." });
        }

        // ----------------------------------------------------------------------------------------------
    }
}
