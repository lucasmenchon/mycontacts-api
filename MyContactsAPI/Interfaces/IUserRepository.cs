using MyContactsAPI.Dtos;
using MyContactsAPI.Models;
using MyContactsAPI.ViewModels;

namespace MyContactsAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User> FindByEmailAsync(string email);
        Task<Response> CreateUserAsync(CreateUserDto createUserDto);
        Task<User> UpdateUserAsync(int id, UpdateUserViewModel userUpdate);
        Task<bool> DeleteUserAsync(int id);
    }
}
