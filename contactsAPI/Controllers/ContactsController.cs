using System;
using Microsoft.AspNetCore.Mvc;
using contactsAPI.Models;
using contactsAPI.Service;
using System.Threading.Tasks;
using System.Linq;

namespace contactsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetContacts()
        {
            var contacts = await _contactService.GetAllContactsAsync();

            if (contacts == null || !contacts.Any())
            {
                return NotFound(new { message = "No contacts found" });
            }
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactById(Guid id)
        {
            var contact = await _contactService.GetContactByIdAsync(id);

            if (contact == null)
            {
                return NotFound(new { message = "Contact not found" });
            }
            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> AddContact([FromBody] AddContactRequest addContactRequest)
        {
            var contact = await _contactService.AddContactAsync(addContactRequest);

            if (contact == null)
            {
                return BadRequest(new { message = "Failed to add contact" });
            }
            return Ok(new { id = contact.Id, message = "Contact added successfully!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact([FromBody] UpdateContactRequest updateContactRequest)
        {
            var contact = await _contactService.GetContactByIdAsync(id);

            if (contact == null)
            {
                return NotFound(new { message = "Contact not found" });
            }

            contact.Name = updateContactRequest.Name;
            contact.Email = updateContactRequest.Email;
            contact.Phone = updateContactRequest.Phone;
            contact.Address = updateContactRequest.Address;

            await _contactService.UpdateContactAsync(contact);

            return Ok(new { id = contact.Id, message = "Contact updated successfully!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            var contact = await _contactService.GetContactByIdAsync(id);

            if (contact == null)
            {
                return NotFound(new { message = "Contact not found" });
            }

            await _contactService.DeleteContactAsync(id);

            return Ok(new { message = "Contact deleted successfully!" });
        }
    }
}
