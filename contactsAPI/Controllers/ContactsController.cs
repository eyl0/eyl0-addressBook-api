using contactsAPI.Data;
using contactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace contactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactsAPIDbContext dbContext;

        public ContactsController(ContactsAPIDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetContacts()
        {
            var contact = await dbContext.Contacts.ToListAsync();

            if (contact == null || contact.Count == 0)
            {
                return NotFound(new { message = "No contacts found" });
            } 
            return Ok(contact);
        }

        [HttpGet]
        [Route("getById/{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound(new { message = "Contact not found, Id does not exist." });
            }
            return Ok(contact);
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddContact([FromBody] AddContactRequest addContactRequest)
        {
            // Validate Name (Required and Length)
            if (string.IsNullOrWhiteSpace(addContactRequest.Name))
            {
                return BadRequest(new { message = "Name is required." });
            }
            else if (addContactRequest.Name.Length < 2 || addContactRequest.Name.Length > 50)
            {
                return BadRequest(new { message = "Name must be between 2 and 50 characters." });
            }
            // Validate Email (Required and Format)
            if (string.IsNullOrWhiteSpace(addContactRequest.Email) || addContactRequest.Email == null)
            {
                return BadRequest(new { message = "Email is required." });
            }
            else if (!IsValidEmail(addContactRequest.Email))
            {
                return BadRequest(new { message = "Invalid email format." });
            }
            // Validate Phone (Required and Format)
            if (string.IsNullOrWhiteSpace(addContactRequest.Phone) || addContactRequest.Phone == null)
            {
                return BadRequest(new { message = "Phone is required." });
            }
            else if (!IsValidPhoneNumber(addContactRequest.Phone))
            {
                return BadRequest(new { message = "Invalid phone number format." });
            }
            // Validate Address (Required)
            if (string.IsNullOrWhiteSpace(addContactRequest.Address) || addContactRequest.Address == null)
            {
                return BadRequest(new { message = "Address is required." });
            }

            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Name = addContactRequest.Name,
                Email = addContactRequest.Email,
                Phone = addContactRequest.Phone,
                Address = addContactRequest.Address
            };

            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();

            return Ok(new { id = contact.Id, message = "Contact added successfully!" });
        }

        [HttpPut]
        [Route("edit/{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest) 
        {

            var contact = await dbContext.Contacts.FindAsync(id);

            if (contact != null)
            {
                // Validate Name (Required and Length)
                if (string.IsNullOrWhiteSpace(updateContactRequest.Name))
                {
                    return BadRequest(new { message = "Name is required." });
                }
                else if (updateContactRequest.Name.Length < 2 || updateContactRequest.Name.Length > 50)
                {
                    return BadRequest(new { message = "Name must be between 2 and 50 characters." });
                }
                // Validate Phone (Required and Format)
                if (string.IsNullOrWhiteSpace(updateContactRequest.Email))
                {
                    return BadRequest(new { message = "Email is required." });
                }
                else if (!IsValidEmail(updateContactRequest.Email))
                {
                    return BadRequest(new { message = "Invalid email format." });
                }
                // Validate Phone (Required and Format)
                if (string.IsNullOrWhiteSpace(updateContactRequest.Phone))
                {
                    return BadRequest(new { message = "Phone is required." });
                }
                else if (!IsValidPhoneNumber(updateContactRequest.Phone))
                {
                    return BadRequest(new { message = "Invalid phone number format." });
                }
                // Validate Address (Required)
                if (string.IsNullOrWhiteSpace(updateContactRequest.Address))
                {
                    return BadRequest(new { message = "Address is required." });
                }

                contact.Name = updateContactRequest.Name;
                contact.Email = updateContactRequest.Email;
                contact.Phone = updateContactRequest.Phone;
                contact.Address = updateContactRequest.Address;

                await dbContext.SaveChangesAsync();

                return Ok(new { id = contact.Id, message = "Contact updated successfully!" });
            }
             return NotFound(new { message = "Contact not found, Id does not exist." });
        }

        [HttpDelete]
        [Route("delete/{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if (contact != null) 
            {
                dbContext.Remove(contact);
                await dbContext.SaveChangesAsync();

                return Ok(new { id = contact.Id, message = "Contact deleted successfully!" });
            }
            return NotFound(new { message = "Contact not found, Id does not exist." });
        }
        private bool IsValidEmail(string email)
        {
            // Use regular expression to check if the email matches the specified format
            if (Regex.IsMatch(email, @"^[a-zA-Z0-9_.+-]+@gmail.com$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Check if the phone number matches the 11-digit format starting with "09"
            if (Regex.IsMatch(phoneNumber, @"^09\d{9}$"))
            {
                return true;
            }
            // Check if the phone number matches the format with country code "+63"
            if (Regex.IsMatch(phoneNumber, @"^\+63\d{10}$"))
            {
                return true;
            }
            // If the phone number does not match any of the valid formats, return false
            return false;
        }
    }
}
