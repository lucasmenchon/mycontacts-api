using MyContactsAPI.Models;

namespace MyContactsAPI.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact> CreateContactAsync(Contact contact);
        Task<List<Contact>> GetUserContactsAsync(Guid userId);
        Task<Contact> UpdateContactAsync(int id, Contact contact);
        Task<bool> DeleteContactAsync(int id);
    }
}
