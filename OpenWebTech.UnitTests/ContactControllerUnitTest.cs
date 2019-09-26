using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OpenWebTech.Controllers;
using OpenWebTech.Infrastructure.ActionResults;
using OpenWebTech.Infrastructure.Exceptions;
using OpenWebTech.Models;
using OpenWebTech.UnitTests.Infrastrcture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace OpenWebTech.UnitTests
{
    public class ContactControllerUnitTest
    {
        [Fact]
        public async void getContactByIdNotFound()
        {
            // Arrange
            var mock = new Mock<IContactsRepository>();
            mock.Setup(rep => rep.retrieveontactAsync(It.IsAny<int>())).Returns(Task<Contact>.Run(() => { return default(Contact); }));
            ContactsController controller = new ContactsController(mock.Object);

            // Act
            NotFoundResult model = await controller.getContact(It.IsAny<int>()) as NotFoundResult;

            // Assert            
            Assert.Equal((int?)HttpStatusCode.NotFound, model.StatusCode);
        }

        [Theory]
        [ClassData(typeof(ContactsTestData))]
        public async void getContactByIdOk(Contact[] contacts)
        {
            // Arrange
            var contactToFind = contacts.SingleOrDefault<Contact>((m) => m.Id == 2);
            var mock = new Mock<IContactsRepository>();
            mock.SetupSequence(rep => rep.retrieveontactAsync(It.IsAny<int>())).Returns(Task<Contact>.Run(() => contactToFind));
            ContactsController controller = new ContactsController(mock.Object);

            // Act            
            OkObjectResult model = await controller.getContact(contactToFind.Id) as OkObjectResult;

            // Assert   
            Assert.Equal((int?)HttpStatusCode.OK, model.StatusCode);
            Assert.Equal(contactToFind, (Contact)model.Value, Comparer.Get<Contact>((m1, m2) => m1.Id == m2.Id));
        }

        [Fact]
        public async void getContactByIdInternalServerError()
        {
            // Arrange
            var mock = new Mock<IContactsRepository>();
            mock.Setup(rep => rep.retrieveontactAsync(It.IsAny<int>())).Throws<NotFoundBusinessEntityException>();
            ContactsController controller = new ContactsController(mock.Object);

            // Act           
            Exception ex = await Assert.ThrowsAsync<NotFoundBusinessEntityException>(async () => await controller.getContact(It.IsAny<int>()));

            // Assert
            Assert.Equal(expected: typeof(NotFoundBusinessEntityException), actual: ex.GetType());
        }

        [Fact]
        public async void createContactCreated()
        {
            // Arrange
            Contact contactCreated =
                new Contact
                {
                    Id = int.MaxValue,
                    FirstName = $"Name {int.MaxValue}",
                    LastName = $"Body {int.MaxValue}"
                };

            var mock = new Mock<IContactsRepository>();

            ContactsController controller = new ContactsController(mock.Object);
            mock.SetupSequence(rep => rep.retrieveontactAsync(It.IsAny<int>())).Returns(Task<Contact>.Run(() => default(Contact)));
            mock.SetupSequence(rep => rep.createContactAsync(contactCreated)).Returns(Task<Contact>.Run(() => contactCreated));

            // Act            
            CreatedResult model = await controller.createContact(contactCreated) as CreatedResult;

            // Assert
            mock.Verify(rep => rep.createContactAsync(It.IsAny<Contact>()), Times.Once);
            Assert.Equal((int?)HttpStatusCode.Created, model.StatusCode);
        }
    }
}
