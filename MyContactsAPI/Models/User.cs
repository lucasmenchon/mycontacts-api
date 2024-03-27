using System.Text.Json.Serialization;

namespace MyContactsAPI.Models
{
    public class User : Entity
    {
        protected User()
        {
        }

        public User(string name, string username, Email email, Password password)
        {
            Name = name;
            Username = username;
            Email = email;
            Password = password;
        }

        public User(string email, string? password = null)
        {
            Email = email;
            Password = new Password(password);
        }

        public string Name { get; set; }
        public string Username { get; set; }        
        public Email Email { get; set; }
        public Password Password { get; private set; } = null!;
        public DateTimeOffset RegisterDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdateDate { get; set; } = null;

        public void ChangePassword(string newPassword)
        {
            Password = new Password(newPassword);
        }        
    }
}
