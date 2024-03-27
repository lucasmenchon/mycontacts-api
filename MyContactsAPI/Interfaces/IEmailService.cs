using System.Threading.Tasks;

namespace MyContactsAPI.Interfaces
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(string email, string verificationCode);
    }
}