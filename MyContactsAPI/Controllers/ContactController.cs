using Microsoft.AspNetCore.Mvc;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models;
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
            //List<Contact> contacts = await _contactRepository.GetUserContactsAsync(_session.FindUserSession().Id);
            return Ok(/*contacts*/);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact(Contact contact)
        {
            try
            {
                //UserViewModel userLogged = _session.FindUserSession();
                //contact.UserId = userLogged.Id;
                await _contactRepository.CreateContactAsync(contact);
                return CreatedAtAction(nameof(GetAllContacts), new { id = contact.Id }, contact);
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Ops!! Não foi possível cadastrar seu contato, tente novamente ou entre em contato com o suporte, detalhes do erro: {error.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] Contact contactUpdate)
        {
            try
            {
                if (id != contactUpdate.Id)
                {
                    return BadRequest("ID do contato não corresponde ao ID fornecido na URL.");
                }

                await _contactRepository.UpdateContactAsync(id, contactUpdate);

                return NoContent();
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Ops!! Não foi possível atualizar seu contato, tente novamente ou entre em contato com o suporte, detalhes do erro: {error.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            try
            {
                bool result = await _contactRepository.DeleteContactAsync(id);
                if (!result)
                {
                    return NotFound("Contato não encontrado.");
                }
                return NoContent();
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Ops!! Não foi possível apagar seu contato, tente novamente ou entre em contato com o suporte, detalhes do erro: {error.Message}");
            }
        }
    }
}
