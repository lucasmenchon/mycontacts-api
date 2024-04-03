using MyContactsAPI.Models.ContactModels;

namespace MyContactsAPI.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact> CreateContactAsync(Contact contact);
        Task<List<Contact>> GetUserContactsAsync();
        Task<Contact> UpdateContactAsync(Guid id, Contact contact);
        Task<bool> DeleteContactAsync(Guid id);
    }
}
