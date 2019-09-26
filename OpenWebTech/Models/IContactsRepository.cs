using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenWebTech.Models
{
    public interface IContactsRepository
    {
        IEnumerable<Contact> retrieveAllContacts();
        Task<IEnumerable<Contact>> retrieveAllContactsAsync();
        long longCount();
        Task<long> longCountAsync();
        Contact retrieveContact(int id);
        Task<Contact> retrieveContactAsync(int id);
        Contact createContact(Contact Contact);
        Task<Contact> createContactAsync(Contact Contact);
        Contact updateContact(int ContactId, Contact Contact);
        Task<Contact> updateContactAsync(int ContactId, Contact Contact);       
        void deleteContact(int id);
        Task deleteContactAsync(int id);
    }
}
