using ContactsManage.Data;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models;
using MyContactsAPI.ViewModels;
using MyContactsAPI.Helper;
using Microsoft.EntityFrameworkCore;

namespace MyContactsAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext dataContext)
        {
            this._context = dataContext;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            user.RegisterDate = DateOnly.FromDateTime(DateTime.Today);
            string userPassword = user.Password.ToString();
            Password password = new Password(userPassword);
            user.ChangePassword(password.Hash);

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var userToDelete = await _context.Users.FindAsync(id);

            if (userToDelete == null)
            {
                return false;
            }

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return _context.Users.FirstOrDefault(user => user.Email == email);
        }

        public async Task<User> UpdateUserAsync(int id, UpdateUserViewModel userUpdate)
        {
            var existingUser = await _context.Users.FindAsync(id);

            if (existingUser == null)
            {
                throw new InvalidOperationException("Usuário não encontrado.");
            }

            existingUser.Name = userUpdate.Name;
            existingUser.Username = userUpdate.Username;
            existingUser.Email = userUpdate.Email;
            existingUser.UpdateDate = DateOnly.FromDateTime(DateTime.Today);

            await _context.SaveChangesAsync();

            return existingUser;
        }
    }
}
