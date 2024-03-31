using MyContactsAPI.Dtos.Password;
using MyContactsAPI.Models.PasswordModels;
using MyContactsAPI.Models.UserModels;

namespace MyContactsAPI.Interfaces
{
    public interface IUserPasswordService
    {
        Task<UserPasswordResponse> ChangeUserPassword(ChangePasswordDto passwordToChange);
        Task<UserPasswordResponse> SendPasswordResetEmail(string email);
    }

}
