using MyContactsAPI.Models.UserModels;

namespace MyContactsAPI.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CellPhone { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
    }
}
