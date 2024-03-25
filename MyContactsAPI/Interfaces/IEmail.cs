using System.Threading.Tasks;

namespace MyContactsAPI.Interfaces
{
    public interface IEmail
    {
        Task<bool> SendEmailAsync(string email, string subject, string message);
    }
}