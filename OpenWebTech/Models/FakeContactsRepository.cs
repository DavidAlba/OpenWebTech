using OpenWebTech.Infrastructure.DatabaseContexts;
using OpenWebTech.Infrastructure.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenWebTech.Models
{
    public class FakeContactsRepository : IContactsRepository
    {
        public object FakeDatabase { get; private set; }

        public Contact createContact(Contact contact) => createContactAsync(contact)?.Result;

        public async Task<Contact> createContactAsync(Contact contact)
        {
            Contact contactFromRepository = await retrieveContactAsync(contact.Id);
            if (contactFromRepository != null) throw new BusinnessDomainException($"The contact {contact.Id} already exists");

            FakeDataBase.getInstance().Contacts.Add(
                    new Contact
                    {
                        Id = contact.Id,
                        FirstName = contact?.FirstName,
                        LastName = contact?.LastName,
                        Address = contact?.Address,
                        Email = contact?.Email
                    }
                );
                
            return await retrieveContactAsync(contact.Id);
        }

        public IEnumerable<Contact> retrieveAllContacts() => retrieveAllContactsAsync()?.Result;

        public Task<IEnumerable<Contact>> retrieveAllContactsAsync() => Task<long>.Run(() => FakeDataBase.getInstance().Contacts.AsEnumerable<Contact>());

        public Contact retrieveContact(int id) => retrieveContactAsync(id)?.Result;

        public async Task<Contact> retrieveContactAsync(int id) => await FakeDataBase.getInstance().Contacts.ToAsyncEnumerable().SingleOrDefault(m => m.Id == id);

        public long longCount() => longCountAsync().Result;

        public async Task<long> longCountAsync() => await FakeDataBase.getInstance().Contacts.ToAsyncEnumerable<Contact>().LongCount<Contact>();

        public void deleteContact(int id) => deleteContactAsync(id)?.Wait();

        public async Task deleteContactAsync(int id)
        {
            Contact contactToDelete = await retrieveContactAsync(id);
            if (contactToDelete == null) throw new NotFoundBusinessEntityException($"The contact {id} doesn't exist.");

            FakeDataBase.getInstance().Contacts.Remove(await retrieveContactAsync(id));
        }

        public Contact updateContact(int contactId, Contact contact) => updateContactAsync(contactId, contact)?.Result;

        public async Task<Contact> updateContactAsync(int contactId, Contact contact)
        {
            Contact contactToUpdate = await retrieveContactAsync(contactId);
            if (contactToUpdate == null) throw new NotFoundBusinessEntityException($"The contact {contactId} doesn't exist.");

            contactToUpdate.FirstName = contact.FirstName;
            contactToUpdate.LastName = contact.LastName;
            contactToUpdate.FullName = contact.FullName;
            contactToUpdate.Address = contact.Address;
            contactToUpdate.Email = contact.Email;
            return await retrieveContactAsync(contactId);
        }
    }
}
