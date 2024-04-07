using MyContactsAPI.Models;
using MyContactsAPI.Models.ContactModels;

namespace MyContactsAPI.Interfaces;

public interface IContactRepository
{
    Task<ApiResponse> CreateContactAsync(ContactDto contact);
    Task<List<ContactDto>> GetUserContactsAsync();
    Task<ApiResponse> UpdateContactAsync(ContactDto contact);
    Task<ApiResponse> DeleteContactAsync(Guid id);
}

