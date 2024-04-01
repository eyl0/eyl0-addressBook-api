using contactsAPI.Models;

namespace contactsAPI.Service
{
    public interface IContactService
    {
        Task<IEnumerable<Contact>> GetAllContactsAsync();
        Task<Contact> GetContactByIdAsync(Guid id);
        Task<Contact> AddContactAsync(AddContactRequest addContactRequest);
        Task<Contact> UpdateContactAsync(Guid id, UpdateContactRequest updateContactRequest);
        Task<Guid> DeleteContactAsync(Guid id);
    }
}
