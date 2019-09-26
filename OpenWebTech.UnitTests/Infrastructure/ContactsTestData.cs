using OpenWebTech.Models;
using System.Collections;
using System.Collections.Generic;

namespace OpenWebTech.UnitTests.Infrastrcture
{
    public class ContactsTestData : IEnumerable<object[]>
    {

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { getContacts() };
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        private IEnumerable<object> getContacts()
        {
            for (int i = 0; i < 5; i++)
                yield return
                        new Contact
                        {
                            Id = i + 1,
                            FirstName = $"Contact Name {i + 1}",
                            LastName = $"Contact LastName {i + 1}",
                            FullName = $"Contact FullName {i + 1}",
                            Address = $"Contact Address {i + 1}",
                            Email = $"Contact Email {i + 1}"
                        };

        }
    }
}
