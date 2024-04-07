using Microsoft.AspNetCore.Mvc;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models.ContactModels;

namespace MyContactsAPI.Controllers;

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
        List<ContactDto> contacts = await _contactRepository.GetUserContactsAsync();
        return Ok(contacts);
    }

    [HttpPost]
    public async Task<IActionResult> CreateContact(ContactDto contact)
    {
        try
        {
            var response = await _contactRepository.CreateContactAsync(contact);
            return Ok(response);
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Oops!! Unable to register your contact, try again or contact support, error details: {error.Message}");
        }
    }

    [HttpPut("UpdateContact={id}")]
    public async Task<IActionResult> UpdateContact([FromBody] ContactDto contactUpdate)
    {
        try
        {
            var response = await _contactRepository.UpdateContactAsync(contactUpdate);
            return Ok(response);
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Oops!! Unable to update your contact, please try again or contact support, error details: {error.Message}");
        }
    }

    [HttpDelete("DeleteContact={id}")]
    public async Task<IActionResult> DeleteContact(Guid id)
    {
        try
        {
            var response = await _contactRepository.DeleteContactAsync(id);
            return Ok(response);
        }
        catch (Exception error)
        {
            return StatusCode(500, $"Oops!! Unable to delete your contact, try again or contact support, error details: {error.Message}");
        }
    }
}

