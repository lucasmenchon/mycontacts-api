using MyContactsAPI.Models.UserModels;

namespace MyContactsAPI.Models.ContactModels;

public class Contact : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CellPhone { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public User? User { get; set; }
}

