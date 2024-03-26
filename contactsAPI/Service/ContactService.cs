using contactsAPI.Data;
using contactsAPI.Models;

namespace contactsAPI.Service
{
    public class ContactService : IContactService
    {
        private readonly ContactsAPIDbContext _context;

        public ContactService(ContactsAPIDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Contact> GetAllContactsAsync()
        {
            var contact = _context.Contacts.ToList();
            return contact;
        }

        public Contact GetContactByIdAsync(Guid id)
        {
            var contact = _context.Contacts.FirstOrDefault(x => x.Id == id);
            return contact;
        }
        /*
        public void AddContactAsync(Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }

            _context.Contacts.Add(contact);
            _context.SaveChanges();
           
        }

        public void UpdateContactAsync(Contact contact)
        {
            _context.Contacts.Update(contact);
            _context.SaveChanges();
           
        }

        public void DeleteContactAsync(Guid id)
        {
        var contact = _context.Contacts.Find(id);
        _context.Contacts.Remove(contact);
        _context.SaveChanges();

    }
        */
    }
}