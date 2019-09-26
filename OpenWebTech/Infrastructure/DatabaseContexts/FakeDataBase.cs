using OpenWebTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenWebTech.Infrastructure.DatabaseContexts
{
    public class FakeDataBase
    {
        private static readonly object padlock = new object();
        private static FakeDataBase _instance;        

        private FakeDataBase()
        {
            Contacts = new List<Contact>();
            foreach (Contact contact in TestData.getContacts())
                Contacts.Add(contact);

            Skills = new List<Skill>();
            foreach (Skill skill in TestData.getSkills())
                Skills.Add(skill);
        }

        public static FakeDataBase getInstance()
        {
            if (_instance == null)
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new FakeDataBase();
                    }
                }
            }

            return _instance;
        }

        public IList<Contact> Contacts { get; private set;  }
        public IList<Skill> Skills { get; private set; }

        private class TestData
        {
            public static IEnumerable<Contact> getContacts()
            {
                for (int i = 0; i < 5; i++)
                {
                    yield return
                        new Contact
                        {
                            Id = i + 1,
                            FirstName = $"Contact Name {i + 1}",
                            LastName = $"Contact LastName {i + 1}",
                            FullName = $"Contact FullName {i + 1}",
                            Address = $"Contact Address {i + 1}",
                            Email = $"Contact Email {i + 1}",
                            Skills = getSkills().ToList<Skill>()
                        };
                }
            }

            public static IEnumerable<Skill> getSkills()
            {
                for (int i = 0; i < 5; i++)
                {
                    yield return
                        new Skill
                        {
                            Id = i + 1,
                            Name = $"Skill Name {i + 1}",
                            Level = $"Skill Level {i + 1}"
                        };
                }
            }
        }
    }
}
