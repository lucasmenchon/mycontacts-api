using MyContactsAPI.Models.UserModels;
using MyContactsAPI.ViewModels;

namespace MyContactsAPI.Interfaces
{
    public interface IUserPasswordService
    {
        Task<bool> ChangeUserPassword(UserViewModel user, string password);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<User> ResetPasswordAsync(PasswordResetViewModel passwordReset);
    }

}
