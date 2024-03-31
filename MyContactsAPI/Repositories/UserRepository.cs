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
using MyContactsAPI.Models.EmailModels;

namespace MyContactsAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly IEmailService _emailService;
        private readonly JwtTokenService _jwtTokenService;

        public UserRepository(DataContext dataContext, IEmailService email, JwtTokenService jwtTokenService)
        {
            this._dataContext = dataContext;
            _emailService = email;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<UserApiResponse> CreateUserAsync(CreateUserDto createUserDto)
        {
            try
            {
                var email = new Email(createUserDto.Email);
                bool existingUser = await _dataContext.Users.AsNoTracking().AnyAsync(u => u.Email.Address == createUserDto.Email);
                if (existingUser)
                    return new UserApiResponse("Este email já está em uso.", 400);

                var password = new Password(createUserDto.Password);
                var user = new User(createUserDto.Name, createUserDto.Username, email, password);

                using (var transaction = _dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        await _dataContext.Users.AddAsync(user);
                        await _dataContext.SaveChangesAsync();

                        await _emailService.SendVerificationEmailAsync(email.Address, email.Verification.Code);

                        await transaction.CommitAsync();

                        return new UserApiResponse("Cadastro realizado com sucesso. Enviamos para seu email o link de ativação da conta.", 201);
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        return new UserApiResponse("Falha no cadastro.", 500);
                    }
                }
            }
            catch
            {
                return new UserApiResponse("Falha ao criar usuário.", 500);
            }
        }

        public async Task<UserApiResponse> UpdateUserAsync(UpdateUserDto userUpdateDto)
        {
            try
            {
                var userIdClaim = _jwtTokenService.GetUserFromJwtToken();
                var userToUpdate = await _dataContext.Users.FindAsync(Guid.Parse(userIdClaim));

                if (userToUpdate == null)
                {
                    return new UserApiResponse("Usuário não encontrado.", 404);
                }

                userToUpdate.Name = userUpdateDto.Name;
                userToUpdate.Username = userUpdateDto.Username;
                userToUpdate.UpdateDate = DateTimeOffset.UtcNow;

                await _dataContext.SaveChangesAsync();

                return new UserApiResponse("Usuário atualizado com sucesso.", 200);
            }
            catch
            {
                return new UserApiResponse("Falha ao atualizar usuário.", 500);
            }
        }

        public async Task<UserApiResponse> DeleteUserAsync()
        {
            try
            {
                var userIdClaim = _jwtTokenService.GetUserFromJwtToken();

                var userToDelete = await _dataContext.Users.FindAsync(Guid.Parse(userIdClaim));

                if (userToDelete == null)
                {
                    return new UserApiResponse("Usuário não encontrado.", 404);
                }

                _dataContext.Users.Remove(userToDelete);
                await _dataContext.SaveChangesAsync();

                return new UserApiResponse("Usuário excluído com sucesso.", 200);
            }
            catch
            {
                return new UserApiResponse("Falha ao excluir usuário.", 500);
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dataContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Address == email);
        }
    }
}
