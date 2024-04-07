
using ContactsManage.Data;
using Microsoft.EntityFrameworkCore;
using MyContactsAPI.Dtos.User;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models;
using MyContactsAPI.Models.EmailModels;
using MyContactsAPI.Models.PasswordModels;
using MyContactsAPI.Models.UserModels;
using MyContactsAPI.Services;
using MyContactsAPI.SharedContext;

namespace MyContactsAPI.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly DataContext _dataContext;

        public AuthRepository(DataContext dataContext, IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _dataContext = dataContext;
        }

        public async Task<ApiResponse> UserSigIn(UserSignInDto userSignIn)
        {
            try
            {
                try
                {
                    var email = new Email(userSignIn.Email);
                }
                catch
                {
                    return new ApiResponse($"Email inválido.", 400);
                }

                User? user = await _userRepository.GetUserByEmailAsync(userSignIn.Email);
                if (user is null)
                    return new ApiResponse("Perfil não encontrado", 404);

                if (!user.Password.Challenge(userSignIn.Password))
                    return new ApiResponse("Usuário ou senha inválidos", 400);

                try
                {
                    if (!user.Email.Verification.IsActive)
                        return new ApiResponse("Conta inativa", 400);
                }
                catch
                {
                    return new ApiResponse("Não foi possível verificar seu perfil", 500);
                }

                try
                {
                    var data = new ResponseData
                    {
                        Token = JwtTokenService.GenerateToken(user)
                    };

                    return new ApiResponse(string.Empty, data);
                }
                catch
                {
                    return new ApiResponse("Não foi possível obter os dados do perfil", 500);
                }
            }
            catch
            {
                return new ApiResponse("Não foi possível encontrar seu perfil", 500);
            }
        }

        public async Task<ApiResponse> CreateUserAsync(CreateUserDto createUserDto)
        {
            try
            {
                var email = new Email(createUserDto.Email);
                bool existingUser = await _dataContext.Users.AsNoTracking().AnyAsync(u => u.Email.Address == createUserDto.Email);
                if (existingUser)
                    return new ApiResponse("Este email já está em uso.", 400);

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

                        return new ApiResponse("Cadastro realizado com sucesso. Enviamos para seu email o link de ativação da conta.", 201);
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        return new ApiResponse("Falha no cadastro.", 500);
                    }
                }
            }
            catch
            {
                return new ApiResponse("Falha ao criar usuário.", 500);
            }
        }
    }
}
