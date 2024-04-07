using MyContactsAPI.Dtos.Password;
using MyContactsAPI.Models;

namespace MyContactsAPI.Interfaces;

public interface IUserPasswordService
{
    Task<ApiResponse> ChangeUserPassword(ChangePasswordDto passwordToChange);
    Task<ApiResponse> SendPasswordResetEmail(string email);
}