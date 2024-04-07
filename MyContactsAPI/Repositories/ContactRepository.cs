using ContactsManage.Data;
using Microsoft.EntityFrameworkCore;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models;
using MyContactsAPI.Models.ContactModels;
using MyContactsAPI.Models.UserModels;
using MyContactsAPI.Services;

namespace MyContactsAPI.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly DataContext _dataContext;
    private readonly JwtTokenService _jwtTokenService;

    public ContactRepository(DataContext bancoContext, JwtTokenService jwtTokenService)
    {
        this._dataContext = bancoContext;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<ApiResponse> CreateContactAsync(ContactDto contactDto)
    {
        try
        {
            var contact = new Contact
            {
                Name = contactDto.Name,
                Email = contactDto.Email,
                CellPhone = contactDto.CellPhone,
                UserId = Guid.Parse(_jwtTokenService.GetUserIdFromJwtToken())
            };

            _dataContext.Contacts.Add(contact);

            await _dataContext.SaveChangesAsync();

            return new ApiResponse("Contact registered successfully!", 201);
        }
        catch
        {
            return new ApiResponse("Failed to create contact.", 500);
        }
    }


    public async Task<ApiResponse> UpdateContactAsync(ContactDto contactDto)
    {
        try
        {
            if (contactDto == null)
                throw new ArgumentNullException(nameof(contactDto));

            var existingContact = await _dataContext.Contacts.FindAsync(contactDto.Id);

            if (existingContact == null)
            {
                throw new InvalidOperationException("Contact not found.");
            }

            existingContact.Name = contactDto.Name;
            existingContact.Email = contactDto.Email;
            existingContact.CellPhone = contactDto.CellPhone;

            await _dataContext.SaveChangesAsync();

            return new ApiResponse("Contact updated successfully.", 201);
        }
        catch
        {
            return new ApiResponse("Failed to update contact.", 500);
        }
    }


    public async Task<ApiResponse> DeleteContactAsync(Guid id)
    {
        try
        {
            var contactToDelete = await _dataContext.Contacts.FindAsync(id);

            if (contactToDelete == null)
            {
                return new ApiResponse("Contact not found.", 404);
            }

            _dataContext.Contacts.Remove(contactToDelete);
            await _dataContext.SaveChangesAsync();

            return new ApiResponse("Contact deleted successfully.", 201);
        }
        catch
        {
            return new ApiResponse("Failed to delete contact.", 500);
        }
    }

    public async Task<List<ContactDto>> GetUserContactsAsync()
    {
        var userIdClaim = _jwtTokenService.GetUserIdFromJwtToken();

        var userContacts = await _dataContext.Contacts
            .Where(c => c.UserId == Guid.Parse(userIdClaim))
            .Select(c => new ContactDto
            {
                Name = c.Name,
                Email = c.Email,
                CellPhone = c.CellPhone
            })
            .ToListAsync();

        return userContacts;
    }
}

