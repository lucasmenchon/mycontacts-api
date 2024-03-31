using ContactsManage.Data;
using Microsoft.EntityFrameworkCore;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models.ContactModels;
using MyContactsAPI.Services;

namespace MyContactsAPI.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly DataContext _context;
        private readonly JwtTokenService _jwtTokenService;

        public ContactRepository(DataContext bancoContext, JwtTokenService jwtTokenService)
        {
            this._context = bancoContext;
            _jwtTokenService = jwtTokenService; 
        }

        public async Task<Contact> CreateContactAsync(Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));

            _context.Contacts.Add(contact);

            await _context.SaveChangesAsync();

            return contact;
        }

        public async Task<Contact> UpdateContactAsync(int id, Contact contact)
        {
            var existingContact = await _context.Contacts.FindAsync(id);

            if (existingContact == null)
            {
                throw new InvalidOperationException("Contato não encontrado.");
            }

            existingContact.Name = contact.Name;
            existingContact.Email = contact.Email;
            existingContact.CellPhone = contact.CellPhone;

            await _context.SaveChangesAsync();

            return existingContact;
        }

        public async Task<bool> DeleteContactAsync(int id)
        {
            var contactToDelete = await _context.Contacts.FindAsync(id);

            if (contactToDelete == null)
            {
                return false;
            }

            _context.Contacts.Remove(contactToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Contact>> GetUserContactsAsync()
        {
            var userIdClaim = _jwtTokenService.GetUserFromJwtToken();
            var userContacts = await _context.Contacts.Where(c => c.UserId == Guid.Parse(userIdClaim)).ToListAsync();
            return userContacts;
        }
    }
}
