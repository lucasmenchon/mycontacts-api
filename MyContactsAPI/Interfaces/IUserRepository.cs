using MyContactsAPI.Dtos.User;
using MyContactsAPI.Models.UserModels;

namespace MyContactsAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<Response> CreateUserAsync(CreateUserDto createUserDto);
        Task<Response> UpdateUserAsync(UpdateUserDto userUpdate);        
        Task<bool> DeleteUserAsync(int id);
    }
}
