using contactsAPI.Models;

namespace contactsAPI.Service
{
    public interface IContactService
    {
        public IEnumerable<Contact> GetAllContactsAsync();
        public Contact GetContactByIdAsync(Guid id);
        //Task<Contact> AddContactAsync(AddContactRequest addContactRequest);
        //Task<Contact> UpdateContactAsync(Guid id, UpdateContactRequest updateContactRequest);
        //Task DeleteContactAsync(Guid id);
    }
}
