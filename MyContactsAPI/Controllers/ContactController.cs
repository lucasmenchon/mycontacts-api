using Microsoft.AspNetCore.Mvc;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models.ContactModels;
using MyContactsAPI.ViewModels;

namespace MyContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private readonly IContactRepository _contactRepository;
        

        public ContactController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;            
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            List<Contact> contacts = await _contactRepository.GetUserContactsAsync();
            return Ok(contacts);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact(Contact contact)
        {
            try
            {
                await _contactRepository.CreateContactAsync(contact);
                return CreatedAtAction(nameof(GetAllContacts), new { id = contact.Id }, contact);
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Oops!! Unable to register your contact, try again or contact support, error details: {error.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(Guid id, [FromBody] Contact contactUpdate)
        {
            try
            {
                if (id != contactUpdate.Id)
                {
                    return BadRequest("Contact ID does not match the ID provided in the URL.");
                }

                await _contactRepository.UpdateContactAsync(id, contactUpdate);

                return NoContent();
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Oops!! Unable to update your contact, please try again or contact support, error details: {error.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            try
            {
                bool result = await _contactRepository.DeleteContactAsync(id);
                if (!result)
                {
                    return NotFound("Contact not found.");
                }
                return NoContent();
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Oops!! Unable to delete your contact, try again or contact support, error details: {error.Message}");
            }
        }
    }
}
