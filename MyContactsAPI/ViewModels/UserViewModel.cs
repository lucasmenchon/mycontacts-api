using MyContactsAPI.Models;

namespace MyContactsAPI.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateOnly RegisterDate { get; set; }
        public DateOnly? UpdateDate { get; set; }
        public virtual List<Contact>? Contacts { get; set; }
    }
}
