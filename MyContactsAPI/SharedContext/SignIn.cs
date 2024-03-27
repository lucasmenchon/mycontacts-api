using System.ComponentModel.DataAnnotations;

namespace MyContactsAPI.SharedContext
{
    public class SignIn
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
