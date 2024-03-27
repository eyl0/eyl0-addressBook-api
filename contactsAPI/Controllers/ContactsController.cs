using Microsoft.AspNetCore.Mvc;
using contactsAPI.Models;
using contactsAPI.Service;

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
        public async Task<ActionResult> GetAllContactsAsync()
        {
            try
            {
                var contact = await _contactService.GetAllContactsAsync();
                return Ok(contact);
            }
            catch (ArgumentNullException err)
            {
                return NotFound(new { error = err.Message });
            }
            catch (Exception err)
            {
                return StatusCode(500, new { error = err.Message });
            }
        }

        [HttpGet("getById/{id}")]
        public async Task<ActionResult<Contact>> GetContactByIdAsync(Guid id)
        {
            try
            {
                var contact = await _contactService.GetContactByIdAsync(id);
                return Ok(contact);
            }
            catch (ArgumentNullException err)
            {
                return NotFound(new { error = err.Message });
            }
            catch (Exception err)
            {
                return StatusCode(500, new { error = err.Message });
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Contact>> AddContactAsync([FromBody] AddContactRequest addContactRequest)
        {
            try
            {
                var contact = await _contactService.AddContactAsync(addContactRequest);
                return Ok(new { id = contact.Id, message = "Contact added successfully!"});
            }
            catch (ArgumentNullException err)
            {
                return BadRequest(new { error = err.Message });
            }
            catch (ArgumentException err)
            {
                return BadRequest(new { message = err.Message });
            }
            catch (Exception err)
            {
                return StatusCode(500, new { error = "InternalError", message = err.Message });
            }
        }


        [HttpPut("edit/{id}")]
        public async Task<ActionResult> UpdateContactAsync([FromRoute] Guid id, [FromBody] UpdateContactRequest updateContactRequest)
        {
            try
            {
                var contact = await _contactService.UpdateContactAsync(id, updateContactRequest);
                return Ok(new { id = contact.Id, message = "Contact updated successfully" });
            }
            catch (ArgumentNullException err)
            {
                return BadRequest(new { error = err.Message });
            }
            catch (ArgumentException err)
            {
                return BadRequest(new { error = err.Message });
            }
            catch (Exception err)
            {
                return StatusCode(500, new { error = err.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteContact(Guid id)
        {
            try
            {
                var deletedContactId = await _contactService.DeleteContactAsync(id);
                return Ok(new { id = deletedContactId, message = "Contact deleted successfully"});
            }
            catch (ArgumentNullException err)
            {
                return NotFound(new { error = err.Message });
            }
            catch (Exception err)
            {
                return StatusCode(500, new { error = err.Message });
            }
        }
    }
}
