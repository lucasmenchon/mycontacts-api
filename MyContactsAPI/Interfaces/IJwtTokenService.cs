using MyContactsAPI.Models;

namespace MyContactsAPI.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
        Guid ValidateJwtToken(string authorizationHeader);
    }
}
