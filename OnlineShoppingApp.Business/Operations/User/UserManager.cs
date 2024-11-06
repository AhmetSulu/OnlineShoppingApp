using OnlineShoppingApp.Business.DataProtection;
using OnlineShoppingApp.Business.Operations.User.Dtos;
using OnlineShoppingApp.Business.Types;
using OnlineShoppingApp.Data.Entities;
using OnlineShoppingApp.Data.Enums;
using OnlineShoppingApp.Data.Repositories;
using OnlineShoppingApp.Data.UnitOfWork;

namespace OnlineShoppingApp.Business.Operations.User
{
    public class UserManager : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IDataProtection _protector;

        public UserManager(IUnitOfWork unitOfWork, IRepository<UserEntity> userRepository, IDataProtection protection)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _protector = protection;
        }

        // ----------------------------------------------------------------------------------------------

        // Adds a new user to the database.
        public async Task<ServiceMessage> AddUser(AddUserDto user)
        {
            // Check if the email is already in use (case-insensitive).
            var hasMail = _userRepository.GetAll(x => x.Email.ToLower() == user.Email.ToLower());

            if (hasMail.Any())
            {
                return new ServiceMessage
                {
                    IsSuccess = false,
                    Message = "This email is already in use." // Return an error if the email exists.
                };
            }

            // Create a new user entity with protected password.
            var userEntity = new UserEntity
            {
                Email = user.Email,
                Password = _protector.Protect(user.Password), // Encrypt the password
                ConfirmPassword = _protector.Protect(user.ConfirmPassword), // Encrypt the confirmation password
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate,
                Address = user.Address,
                UserType = UserType.Customer // Set user type to Customer
            };
            _userRepository.Add(userEntity); // Add the new user entity to the repository.

            try
            {
                await _unitOfWork.SaveChangesAsync(); // Save changes to the database.
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while adding the user."); // Handle any exceptions.
            }

            return new ServiceMessage
            {
                IsSuccess = true, // Return success message
            };
        }

        // ----------------------------------------------------------------------------------------------

        // Logs in a user by checking email and password.
        public ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user)
        {
            // Retrieve user by email (case-insensitive).
            var userEntity = _userRepository.Get(x => x.Email.ToLower() == user.Email.ToLower());

            if (userEntity is null)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSuccess = false,
                    Message = "User not found." // Return error if user doesn't exist.
                };
            }

            // Decrypt the stored password.
            var unprotectedPassword = _protector.UnProtect(userEntity.Password);

            // Check if the provided password matches the stored password.
            if (unprotectedPassword == user.Password)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSuccess = true,
                    Data = new UserInfoDto()
                    {
                        Id = userEntity.Id,
                        Email = userEntity.Email,
                        FirstName = userEntity.FirstName,
                        LastName = userEntity.LastName,
                        UserType = userEntity.UserType // Return user info if login is successful.
                    }
                };
            }
            else
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSuccess = false,
                    Message = "Incorrect password or email." // Return error if credentials are incorrect.
                };
            }
        }

        // ----------------------------------------------------------------------------------------------

        // Changes a user's password.
        public async Task<ServiceMessage> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            // Retrieve user by email (case-insensitive).
            var userEntity = _userRepository.Get(x => x.Email.ToLower() == changePasswordDto.Email.ToLower());

            if (userEntity is null)
            {
                return new ServiceMessage
                {
                    IsSuccess = false,
                    Message = "User not found." // Return error if user doesn't exist.
                };
            }

            // Decrypt the stored password to verify the old password.
            var unprotectedOldPassword = _protector.UnProtect(userEntity.Password);
            if (unprotectedOldPassword != changePasswordDto.OldPassword)
            {
                return new ServiceMessage
                {
                    IsSuccess = false,
                    Message = "Old password is incorrect." // Return error if the old password is incorrect.
                };
            }

            // Check if the new password matches the confirmation password.
            if (changePasswordDto.NewPassword != changePasswordDto.ConfirmNewPassword)
            {
                return new ServiceMessage
                {
                    IsSuccess = false,
                    Message = "New password and confirmation do not match." // Return error if they don't match.
                };
            }

            // Update the password with the new protected password.
            userEntity.Password = _protector.Protect(changePasswordDto.NewPassword);
            _userRepository.Update(userEntity); // Update the user in the repository.

            try
            {
                await _unitOfWork.SaveChangesAsync(); // Save changes to the database.
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while changing the password."); // Handle any exceptions.
            }

            return new ServiceMessage
            {
                IsSuccess = true,
                Message = "Password changed successfully." // Return success message.
            };
        }

        // ----------------------------------------------------------------------------------------------
    }
}
