namespace MyContactsAPI.Models.ContactModels
{
    public class ContactDto : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CellPhone { get; set; } = string.Empty;
    }
}
