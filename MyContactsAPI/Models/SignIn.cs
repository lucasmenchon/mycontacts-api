using System.ComponentModel.DataAnnotations;

namespace MyContactsAPI.Models
{
    public class SignIn
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
