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
using Microsoft.AspNetCore.Http;

namespace MyContactsAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(DataContext dataContext, IEmailService email, IHttpContextAccessor httpContextAccessor)
        {
            this._context = dataContext;
            _emailService = email;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response> CreateUserAsync(CreateUserDto createUserDto)
        {
            try
            {
                var email = new Email(createUserDto.Email);
                bool existingUser = await _context.Users.AsNoTracking().AnyAsync(u => u.Email.Address == createUserDto.Email);
                if (existingUser)
                    return new Response("Este email já está em uso.", 400);

                var password = new Password(createUserDto.Password);
                var user = new User(createUserDto.Name, createUserDto.Username, email, password);

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        await _context.Users.AddAsync(user);
                        await _context.SaveChangesAsync();

                        await _emailService.SendVerificationEmailAsync(email.Address, email.Verification.Code);

                        await transaction.CommitAsync();

                        return new Response("Cadastro realizado com sucesso. Enviamos para seu email o link de ativação da conta.", 201);
                    }
                    catch
                    {
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

        public async Task<Response> DeleteUserAsync()
        {
            try
            {
                var userToDelete = await new JwtTokenService().GetUserFromJwtTokenAsync(_context);

                if (userToDelete == null)
                {
                    return new Response("Usuário não encontrado.", 404);
                }

                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();

                return new Response("Usuário excluído com sucesso.", 200);
            }
            catch
            {
                return new Response("Falha ao excluir usuário.", 500);
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Address == email);
        }

        public async Task<Response> UpdateUserAsync(UpdateUserDto userUpdateDto)
        {
            try
            {
                var userToUpdate = await new JwtTokenService().GetUserFromJwtTokenAsync(_context);

                if (userToUpdate == null)
                {
                    return new Response("Usuário não encontrado.", 404);
                }

                userToUpdate.Name = userUpdateDto.Name;
                userToUpdate.Username = userUpdateDto.Username;
                userToUpdate.UpdateDate = DateTimeOffset.UtcNow;

                await _context.SaveChangesAsync();

                return new Response("Usuário atualizado com sucesso.", 200);
            }
            catch
            {
                return new Response("Falha ao atualizar usuário.", 500);
            }
        }
    }
}
