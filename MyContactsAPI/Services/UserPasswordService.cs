using ContactsManage.Data;
using MyContactsAPI.Helper;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models.PasswordModels;
using MyContactsAPI.Models.UserModels;
using MyContactsAPI.ViewModels;

namespace MyContactsAPI.Services
{
    public class UserPasswordService : IUserPasswordService
    {
        private readonly DataContext _context;

        public UserPasswordService(DataContext dataContext)
        {
            this._context = dataContext;
        }

        public async Task<bool> ChangeUserPassword(UserViewModel userLogged, string password)
        {
            if (userLogged == null || string.IsNullOrEmpty(password))
            {
                return await Task.FromResult(false);
            }

            // Busca o usuário pelo ID
            User user = await _context.Users.FindAsync(userLogged.Id);

            // Verifica se o usuário foi encontrado
            if (user == null)
            {
                return await Task.FromResult(false);
            }

            // Verifica se a senha fornecida corresponde à senha do usuário
            bool passwordMatch = user.Password.Challenge(password);

            return await Task.FromResult(passwordMatch);
        }


        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            if (user == null || string.IsNullOrEmpty(password))
            {
                return await Task.FromResult(false);
            }

            // Crie uma instância da classe Password com a senha armazenada no usuário
            Password userPassword = new Password(user.Password.ToString());

            // Verifique se a senha fornecida coincide com o hash armazenado no usuário
            bool passwordMatch = userPassword.Challenge(password);

            return await Task.FromResult(passwordMatch);
        }


        public async Task<User> ResetPasswordAsync(PasswordResetViewModel passwordReset)
        {
            // Busca o usuário pelo ID
            User user = await _context.Users.FindAsync(passwordReset.Id);

            if (user == null)
            {
                throw new ArgumentException("Usuário não encontrado.");
            }

            // Cria uma instância da classe Password com a senha armazenada no usuário
            Password userPassword = new Password(user.Password.ToString());

            // Verifica se a nova senha é igual à senha antiga
            if (userPassword.Challenge(passwordReset.NewPassword))
            {
                throw new ArgumentException("A nova senha deve ser diferente da senha antiga.");
            }

            // Cria um novo hash para a nova senha
            Password newPassword = new Password(passwordReset.NewPassword);

            // Verifica se o hash da nova senha foi gerado com sucesso
            if (string.IsNullOrEmpty(newPassword.Hash))
            {
                throw new ArgumentException("Falha ao gerar o hash da nova senha.");
            }

            user.ChangePassword(newPassword.Hash);

            // Salva as alterações no banco de dados
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }




    }
}
