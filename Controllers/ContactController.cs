﻿using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllContacts()
    {
        try
        {
            List<ContactDto> contacts = await _contactRepository.GetUserContactsAsync();
            return Ok(contacts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Oops!! Unable to get your contact, please try again or contact support, error details: {ex.Message}");
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateContact(ContactDto contact)
    {
        try
        {
            var response = await _contactRepository.CreateContactAsync(contact);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Oops!! Unable to register your contact, try again or contact support, error details: {ex.Message}");
        }
    }

    [Authorize]
    [HttpPut("UpdateContact={id}")]
    public async Task<IActionResult> UpdateContact([FromBody] ContactDto contactUpdate)
    {
        try
        {
            var response = await _contactRepository.UpdateContactAsync(contactUpdate);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Oops!! Unable to update your contact, please try again or contact support, error details: {ex.Message}");
        }
    }

    [Authorize]
    [HttpDelete("DeleteContact={id}")]
    public async Task<IActionResult> DeleteContact(Guid id)
    {
        try
        {
            var response = await _contactRepository.DeleteContactAsync(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Oops!! Unable to delete your contact, try again or contact support, error details: {ex.Message}");
        }
    }
}

