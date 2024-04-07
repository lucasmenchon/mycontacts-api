using MyContactsAPI.Models.ContactModels;

namespace MyContactsAPI.Dtos.User;

public class UserDto
{
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public virtual List<ContactDto>? Contacts { get; set; }
}

