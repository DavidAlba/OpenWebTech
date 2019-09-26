using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenWebTech.Models
{
    public interface ISkillsRepository
    {
        IEnumerable<Skill> retrieveAllSkills();
        Task<IEnumerable<Skill>> retrieveAllSkillsAsync();
        long longCount();
        Task<long> longCountAsync();
        Skill retrieveSkill(int id);
        Task<Skill> retrieveSkillAsync(int id);
        Skill createSkill(Skill Skill);
        Task<Skill> createSkillAsync(Skill skill);
        Skill updateSkill(int skillId, Skill skill);
        Task<Skill> updateSkillAsync(int skillId, Skill skill);
        void deleteSkill(int skillId);
        Task deleteSkillAsync(int skillId);
        

        Contact addSkillToContact(int skillId, int contactId);
        Contact removeSkillFromContact(int skillId, int contactId);
    }
}
