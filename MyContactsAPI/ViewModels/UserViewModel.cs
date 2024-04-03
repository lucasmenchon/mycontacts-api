using MyContactsAPI.Models.ContactModels;

namespace MyContactsAPI.ViewModels
{
    public class UserViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public virtual List<Contact>? Contacts { get; set; }
    }
}
