using ContactsManage.Data;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models;
using MyContactsAPI.ViewModels;
using MyContactsAPI.Helper;
using Microsoft.EntityFrameworkCore;
using MyContactsAPI.Dtos;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Threading;
using MyContactsAPI.Services;

namespace MyContactsAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IEmailService _emailService;

        public UserRepository(DataContext dataContext, IEmailService email)
        {
            this._context = dataContext;
            _emailService = email;
        }

        public async Task<Response> CreateUserAsync(CreateUserDto createUserDto)
        {
            Email email;
            Password password;
            User user;

            try
            {
                email = new Email(createUserDto.Email);
                password = new Password(createUserDto.Password);
                user = new User(createUserDto.Name, createUserDto.Username, email, password);
            }
            catch (Exception ex)
            {
                return new Response(ex.Message, 400);
            }

            try
            {
                bool existingUser = await _context.Users.AsNoTracking().AnyAsync(u => u.Email.Address == createUserDto.Email);
                if (existingUser)
                    return new Response("Este email já está em uso.", 400);
            }
            catch
            {
                return new Response("Falha ao verificar email cadastrado.", 500);
            }

            try
            {
                await _context.Users.AddAsync(user);

                await _context.SaveChangesAsync();
            }
            catch
            {
                return new Response("Falha no cadastro.", 500);
            }

            try
            {
                await _emailService.SendVerificationEmailAsync(email.Address, email.Verification.Code);
            }
            catch
            {
                return new Response("Falha ao enviar código de verificação.", 500);
            }

            return new Response("Cadastro realizado com sucesso. Enviamos para seu email o link de ativação da sua conta.", 201);
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
            //existingUser.UpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return existingUser;
        }
    }
}
