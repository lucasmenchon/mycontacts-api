using MyContactsAPI.Enums;
using MyContactsAPI.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MyContactsAPI.Models
{
    public class User : Entity
    {
        protected User()
        {
        }
        public User(string name, Email email, Password password)
        {
            Name = name;
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
        public Password Password { get; private set; } = null!;
        public Email Email { get; set; }
        public DateOnly RegisterDate { get; set; }
        public DateOnly? UpdateDate { get; set; }

        public void ChangePassword(string newPassword)
        {
            Password = new Password(newPassword);
        }

    }
}
