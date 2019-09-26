using Microsoft.AspNetCore.Http;
using OpenWebTech.Infrastructure.DatabaseContexts;
using OpenWebTech.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenWebTech.Models
{
    public class FakeSkillsRepository : ISkillsRepository
    {
        public Contact addSkillToContact(int skillId, int contactId)
        {
            Skill skill = FakeDataBase.getInstance().Skills.FirstOrDefault<Skill>(s => s.Id == skillId);
            if (skill == null) throw new BusinnessDomainException($"The skill {skillId} does not exist");

            Contact contact = FakeDataBase.getInstance().Contacts.FirstOrDefault<Contact>(c => c.Id == contactId);
            if (contact == null) throw new BusinnessDomainException($"The contact {contactId} does not exist");

            if (!contact.Skills.Contains(skill)) contact.Skills.Add(skill);

            return contact;
        }

        public Contact removeSkillFromContact(int skillId, int contactId)
        {
            Skill skill = FakeDataBase.getInstance().Skills.FirstOrDefault<Skill>(s => s.Id == skillId);
            if (skill == null) throw new BusinnessDomainException($"The skill {skillId} does not exist");

            Contact contact = FakeDataBase.getInstance().Contacts.FirstOrDefault<Contact>(c => c.Id == contactId);
            if (contact == null) throw new BusinnessDomainException($"The contact {contactId} does not exist");

            if (contact.Skills.Contains(skill)) contact.Skills.Remove(skill);

            return contact;
        }

        public Skill createSkill(Skill skill) => createSkillAsync(skill)?.Result;

        public async Task<Skill> createSkillAsync(Skill skill)
        {
            Skill skillFromRepository = await retrieveSkillAsync(skill.Id);
            if (skillFromRepository != null) throw new BusinnessDomainException($"The contact {skill.Id} already exists");

            FakeDataBase.getInstance().Skills.Add(
                    new Skill
                    {
                        Id = skill.Id,
                        Name = skill.Name,
                        Level = skill.Level
                    }
                );

            return await retrieveSkillAsync(skill.Id);
        }

        public void deleteSkill(int skillId) => deleteSkillAsync(skillId)?.Wait();

        public async Task deleteSkillAsync(int skillId)
        {
            Skill skillFromRepository = await retrieveSkillAsync(skillId);
            if (skillFromRepository == null) throw new NotFoundBusinessEntityException($"The skill {skillId} doesn't exist.");

            FakeDataBase.getInstance().Skills.Remove(await retrieveSkillAsync(skillId));
        }

        public long longCount() => longCountAsync().Result;

        public async Task<long> longCountAsync() => await FakeDataBase.getInstance().Skills.ToAsyncEnumerable<Skill>().LongCount<Skill>();      

        public IEnumerable<Skill> retrieveAllSkills() => retrieveAllSkillsAsync()?.Result;

        public Task<IEnumerable<Skill>> retrieveAllSkillsAsync() => Task<long>.Run(() => FakeDataBase.getInstance().Skills.AsEnumerable<Skill>());

        public Skill retrieveSkill(int id) => retrieveSkillAsync(id)?.Result;

        public async Task<Skill> retrieveSkillAsync(int id) => await FakeDataBase.getInstance().Skills.ToAsyncEnumerable().SingleOrDefault(m => m.Id == id);

        public Skill updateSkill(int skillId, Skill skill) => updateSkillAsync(skillId, skill)?.Result;

        public async Task<Skill> updateSkillAsync(int skillId, Skill skill)
        {
            Skill skillToUpdate = await retrieveSkillAsync(skillId);
            if (skillToUpdate == null) throw new NotFoundBusinessEntityException($"The contact {skillId} doesn't exist.");

            skillToUpdate.Name = skill.Name;
            skillToUpdate.Level = skill.Level;
            return await retrieveSkillAsync(skillId);
        }
    }
}
