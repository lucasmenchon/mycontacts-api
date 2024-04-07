using ContactsManage.Data;
using MyContactsAPI.Dtos.Password;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models;
using MyContactsAPI.Models.EmailModels;
using MyContactsAPI.Models.PasswordModels;

namespace MyContactsAPI.Services
{
    public class UserPasswordService : IUserPasswordService
    {
        private readonly DataContext _dataContext;
        private readonly JwtTokenService _jwtTokenService;
        private readonly IUserRepository _userRepository;

        public UserPasswordService(DataContext dataContext, JwtTokenService jwtTokenService, IUserRepository userRepository)
        {
            _dataContext = dataContext;
            _jwtTokenService = jwtTokenService;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse> ChangeUserPassword(ChangePasswordDto passwordToChange)
        {
            var userIdClaim = _jwtTokenService.GetUserIdFromJwtToken();
            var user = await _dataContext.Users.FindAsync(Guid.Parse(userIdClaim));

            if (user == null)
            {
                return new ApiResponse("User not found.", 404);
            }

            Password userPassword = new Password(user.Password.ToString());

            if (!userPassword.Challenge(passwordToChange.OldPassword))
            {
                return new ApiResponse("The old password provided is incorrect.", 400);
            }

            if (userPassword.Challenge(passwordToChange.NewPassword))
            {
                return new ApiResponse("The new password must be different from the old password.", 400);
            }

            if (passwordToChange.NewPassword != passwordToChange.ConfirmNewPassword)
            {
                return new ApiResponse("The new password does not match the confirmation password.", 400);
            }

            Password newPassword = new Password(passwordToChange.NewPassword);

            user.ChangePassword(newPassword.Hash);

            _dataContext.Users.Update(user);
            await _dataContext.SaveChangesAsync();

            return new ApiResponse("Password changed successfully!", 201);
        }

        public async Task<ApiResponse> SendPasswordResetEmail(string email)
        {
            try
            {
                email = new Email(email);
            }
            catch
            {
                return new ApiResponse($"Invalid email.", 400);
            }

            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return new ApiResponse("User not found for the email provided.", 403);
            }

            var resetLink = $"www.lucas.tf/reset-password?token={JwtTokenService.GenerateToken(user)}";

            await new EmailService().SendVerificationEmailAsync(email, resetLink);

            return new ApiResponse("Password reset email sent successfully.", 201);
        }
    }
}
