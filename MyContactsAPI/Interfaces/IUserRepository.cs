using MyContactsAPI.Models;
using MyContactsAPI.ViewModels;

namespace MyContactsAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User> FindByEmailAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(int id, UpdateUserViewModel userUpdate);
        Task<bool> DeleteUserAsync(int id);
    }
}
