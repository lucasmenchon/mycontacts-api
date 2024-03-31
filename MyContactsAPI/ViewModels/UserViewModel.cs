using MyContactsAPI.Models.ContactModels;

namespace MyContactsAPI.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateOnly RegisterDate { get; set; } 
        public DateOnly? UpdateDate { get; set; }
        public virtual List<Contact>? Contacts { get; set; }
    }
}
