using ContactsManage.Data;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models;
using Microsoft.EntityFrameworkCore;
using MyContactsAPI.Dtos.User;
using MyContactsAPI.Models.UserModels;
using MyContactsAPI.Models.PasswordModels;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Threading;
using System.Security.Claims;
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
            try
            {
                // Validar o email antes de criar o objeto User
                var email = new Email(createUserDto.Email);
                bool existingUser = await _context.Users.AsNoTracking().AnyAsync(u => u.Email.Address == createUserDto.Email);
                if (existingUser)
                    return new Response("Este email já está em uso.", 400);

                var password = new Password(createUserDto.Password);
                var user = new User(createUserDto.Name, createUserDto.Username, email, password);

                // Começar a transação
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        await _context.Users.AddAsync(user);
                        await _context.SaveChangesAsync();

                        // Enviar e-mail de verificação
                        await _emailService.SendVerificationEmailAsync(email.Address, email.Verification.Code);

                        // Commit da transação se tudo ocorrer bem
                        await transaction.CommitAsync();

                        return new Response("Cadastro realizado com sucesso. Enviamos para seu email o link de ativação da sua conta.", 201);
                    }
                    catch
                    {
                        // Reverter a transação em caso de falha
                        await transaction.RollbackAsync();
                        return new Response("Falha no cadastro.", 500);
                    }
                }
            }
            catch
            {
                return new Response("Falha ao criar usuário.", 500);
            }
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

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Address == email);
        }

        public async Task<Response> UpdateUserAsync(UpdateUserDto userUpdate)
        {
            var userIdClaim = _context.Users.FindAsync(ClaimTypes.NameIdentifier);            

            var existingUser = await _context.Users.FindAsync(userIdClaim);

            if (existingUser == null)
            {
                throw new InvalidOperationException("Usuário não encontrado.");
            }

            existingUser.Name = userUpdate.Name;
            existingUser.Username = userUpdate.Username;
            existingUser.UpdateDate = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();

            return new Response("", 201);
        }
    }
}
