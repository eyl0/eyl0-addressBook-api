using contactsAPI.Data;
using contactsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace contactsAPI.Service
{
    public class ContactService : IContactService
    {
        private readonly ContactsAPIDbContext _context;

        public ContactService(ContactsAPIDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync()
        {
            var contact = await _context.Contacts.ToListAsync();
            
            if (contact == null)
            {
                throw new ArgumentException("Contacts not found.");
            }
            return contact;
        }

        public async Task<Contact> GetContactByIdAsync(Guid id)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == id);

            if (contact == null)
            {
                throw new ArgumentException("Id does not exist.");            
            } 
            return contact;
        }

        public async Task<Contact> AddContactAsync(AddContactRequest addContactRequest)
        {
            if (string.IsNullOrWhiteSpace(addContactRequest.Name))
            {
                throw new ArgumentNullException("Name is required.");
            }
            else if (addContactRequest.Name.Length < 2 || addContactRequest.Name.Length > 50)
            {
                throw new ArgumentException("Name must be between 2 and 50 characters.");
            }
            if (string.IsNullOrWhiteSpace(addContactRequest.Email))
            {
                throw new ArgumentNullException("Email is required.");
            }
            else if (!addContactRequest.Email.EndsWith("@gmail.com"))
            {
                throw new ArgumentException("Invalid email format. Must be in @gmail.com format");
            }
            if (string.IsNullOrWhiteSpace(addContactRequest.Phone))
            {
                throw new ArgumentNullException("Phone is required.");
            }
            else if (!(addContactRequest.Phone.StartsWith("09") && addContactRequest.Phone.Length == 11) && !(addContactRequest.Phone.StartsWith("+63") && addContactRequest.Phone.Length == 11))
            {
                throw new ArgumentException("Invalid phone number format. Must be in 11-digit format starting with '09' or 10-digit format starting with '+63'");
            }
            if (string.IsNullOrWhiteSpace(addContactRequest.Address))
            {
                throw new ArgumentNullException("Address is required.");
            }

            var contact = new Contact
            {
                Name = addContactRequest.Name,
                Email = addContactRequest.Email,
                Phone = addContactRequest.Phone,
                Address = addContactRequest.Address
            };

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return contact;
        }

        public async Task<Contact> UpdateContactAsync(Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact), "Contact not Found");
            }

            if (string.IsNullOrWhiteSpace(updateContactRequest.Name))
            {
                throw new ArgumentNullException(nameof(updateContactRequest.Name), "Name is required.");
            }
            else if (updateContactRequest.Name.Length < 2 || updateContactRequest.Name.Length > 50)
            {
                throw new ArgumentException("Name must be between 2 and 50 characters.", nameof(updateContactRequest.Name));
            }
            if (string.IsNullOrWhiteSpace(updateContactRequest.Email))
            {
                throw new ArgumentNullException(nameof(updateContactRequest.Email), "Email is required.");
            }
            else if (!updateContactRequest.Email.EndsWith("@gmail.com"))
            {
                throw new ArgumentException("Invalid email format. Must be in @gmail.com format", nameof(updateContactRequest.Email));
            }
            if (string.IsNullOrWhiteSpace(updateContactRequest.Phone))
            {
                throw new ArgumentNullException(nameof(updateContactRequest.Phone), "Phone is required.");
            }
            else if (!(updateContactRequest.Phone.StartsWith("09") && updateContactRequest.Phone.Length == 11) && !(updateContactRequest.Phone.StartsWith("+63") && updateContactRequest.Phone.Length == 11))
            {
                throw new ArgumentException("Invalid phone number format. Must be in 11-digit format starting with '09' or 10-digit format starting with '+63'", nameof(updateContactRequest.Phone));
            }
            if (string.IsNullOrWhiteSpace(updateContactRequest.Address))
            {
                throw new ArgumentNullException(nameof(updateContactRequest.Address), "Address is required.");
            }

            contact.Name = updateContactRequest.Name;
            contact.Email = updateContactRequest.Email;
            contact.Phone = updateContactRequest.Phone;
            contact.Address = updateContactRequest.Address;

            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();

            return contact;
        }

        public async Task<Guid> DeleteContactAsync(Guid id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                throw new ArgumentException("Contact not found.");
            }

            var deletedContactId = contact.Id;

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return deletedContactId;
        }
    }
}
