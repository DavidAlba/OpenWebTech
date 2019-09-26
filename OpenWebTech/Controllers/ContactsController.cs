using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OpenWebTech.Infrastructure.Exceptions;
using OpenWebTech.Models;

namespace OpenWebTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private IContactsRepository _repository;

        public ContactsController(IContactsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet(Name = "getAllContactsRoute")]
        [ProducesResponseType(typeof(IEnumerable<Contact>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> retrieveAllContacts() => Ok(await _repository.retrieveAllContactsAsync());

        [HttpPost(Name = "createContactRoute")]
        [ProducesResponseType(typeof(Contact), (int)HttpStatusCode.Created)]        
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> createContact([FromBody]Contact contact)
        {            
            Contact newContact = await _repository.createContactAsync(contact);

            string locationHead = string.Empty;
            if (Url != null) locationHead = Url.Link("createContactRoute", new { id = newContact.Id });

            return Created(
                uri: locationHead,
                value: newContact);
        }

        [HttpGet("{id}", Name = "getContactRoute")]
        [ProducesResponseType(typeof(Contact), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> retrieveContact(int id) {

            IActionResult result = null;

            Contact contact = await _repository.retrieveContactAsync(id);
            if(contact != null)
            {
                result = Ok(contact);
            }
            else
            {
                result = NotFound();
            }

            return result;
        }

        [HttpPut("{id}", Name = "updateContactRoute")]
        [ProducesResponseType(typeof(Contact), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> updateContact([FromBody] Contact contact)
        {
            // HttpStatusCode.Conflict not implemented. Only mentioned to discuss
            return Ok(
                    await _repository.updateContactAsync(
                        contact.Id,
                        contact
                    )
                );
        }

        [HttpDelete("{id}", Name = "deleteContactRoute")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.MethodNotAllowed)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> deleteContact(int id)
        {
            // HttpStatusCode.MethodNotAllowed not implemented. Only mentioned to discuss
            await _repository.deleteContactAsync(id);
            return NoContent();
        }
    }
}
