using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using OpenWebTech.Infrastructure.ActionResults;
using OpenWebTech.Infrastructure.Exceptions;
using OpenWebTech.Infrastructure.Filters;
using OpenWebTech.Models;

namespace OpenWebTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private ISkillsRepository _skillsRepository;
        private IContactsRepository _contactRepository;

        public SkillsController(ISkillsRepository skillsRepository, IContactsRepository contactsRepository)
        {
            _skillsRepository = skillsRepository ?? throw new ArgumentNullException(nameof(skillsRepository));
            _contactRepository = contactsRepository ?? throw new ArgumentNullException(nameof(contactsRepository));
        }

        [Route("{skillId:int}/contacts/{contactId:int}", Name = "addSkillToUserRoute")]
        [HttpPut]
        [ProducesResponseType(typeof(Contact), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult addSkillToUser(int skillId, int contactId)
        {
            IActionResult result = null;

            try
            {
                if (contactId <= 0) return BadRequest($"The contact id is invalid. Id must be greater than zero");
                if (skillId <= 0) return BadRequest($"The skill id is invalid. Id must be greater than zero");

                result = Ok(_skillsRepository.addSkillToContact(skillId, contactId));
            }
            catch (Exception e)
            {
                // Internal Server error
                result = StatusCode(
                    statusCode: StatusCodes.Status500InternalServerError,
                    value: e.Message);
            }

            return result;
        }

        [Route("{skillId:int}/contact/{contactId:int}", Name = "deleteSkillFromContactRoute")]
        [HttpDelete]
        [ProducesResponseType(typeof(Contact), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult deleteSkillFromContact(int skillId, int contactId)
        {
            IActionResult result = null;

            try
            {
                if (contactId <= 0) return BadRequest($"The contact id is invalid. Id must be greater than zero");
                if (skillId <= 0) return BadRequest($"The skill id is invalid. Id must be greater than zero");

                result = Ok(_skillsRepository.removeSkillFromContact(skillId, contactId));
            }
            catch (Exception e)
            {
                // Internal Server error
                result = StatusCode(
                    statusCode: StatusCodes.Status500InternalServerError,
                    value: e.Message);
            }

            return result;
        }

        [HttpGet(Name = "retrieveAllSkillssRoute")]
        [ProducesResponseType(typeof(IEnumerable<Contact>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> retrieveAllSkills()
        {
            IActionResult result = null;

            try
            {                
                result = Ok(await _skillsRepository.retrieveAllSkillsAsync());
            }
            catch (Exception e)
            {
                // Internal Server error
                result = StatusCode(
                    statusCode: StatusCodes.Status500InternalServerError,
                    value: e.Message);
            }

            return result;
        }

        [HttpPost(Name = "createSkillRoute")]
        [ProducesResponseType(typeof(Contact), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> createSkill([FromBody]Skill skill)
        {
            IActionResult result = null;

            try
            {
                if (skill == null) return BadRequest($"The skill is invalid (null)");
                if (skill.Id <= 0) return BadRequest($"The id is invalid. Id must be greater than zero");

                Skill newSkill = await _skillsRepository.createSkillAsync(skill);
                string locationHead = string.Empty;
                if (Url != null) locationHead = Url.Link("createSkillRoute", new { skillId = newSkill.Id });

                result = Created(
                    uri: locationHead,
                    value: newSkill);
            }
            catch (Exception e)
            {
                // Internal Server error
                result = StatusCode(
                    statusCode: StatusCodes.Status500InternalServerError,
                    value: e.Message);
            }

            return result;
        }

        [HttpGet("{skillId}", Name = "retrieveSkillRoute")]
        [ProducesResponseType(typeof(Contact), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> retrieveSkill(int skillId)
        {
            IActionResult result = null;
            Skill skill = null;

            try
            {
                
                skill = await _skillsRepository.retrieveSkillAsync(skillId);
                if (skill != null)
                {
                    result = Ok(skill);
                }
                else
                {
                    result = NotFound();
                }
            }
            catch (Exception e)
            {
                // Internal Server error
                result = StatusCode(
                    statusCode: StatusCodes.Status500InternalServerError,
                    value: e.Message);
            }

            return result;
        }

        [HttpPut("{skillId}", Name = "updateSkillRoute")]
        [ProducesResponseType(typeof(Contact), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> updateSkill([FromBody] Skill skill)
        {
            IActionResult result = null;

            try
            {
                if (skill == null) return BadRequest($"The skill is invalid (null)");
                if (skill.Id <= 0) return BadRequest($"The id is invalid. Id must be greater than zero");

                if (await _skillsRepository.retrieveSkillAsync(skill.Id) != null)
                {
                    result = Ok(await _skillsRepository.updateSkillAsync(skill.Id, skill));
                }
                else
                {
                    result = NotFound($"The skill with {skill.Id} was not found");
                }
            }
            catch (Exception e)
            {
                // Internal Server error
                result = StatusCode(
                    statusCode: StatusCodes.Status500InternalServerError,
                    value: e.Message);
            }

            return result;
        }

        [HttpDelete("{skillId}", Name = "deleteSkillRoute")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.MethodNotAllowed)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> deleteSkill(int skillId)
        {
            IActionResult result = null;

            try
            {
                if (skillId <= 0)
                {
                    result = BadRequest($"The id {skillId} is not valid");
                }
                else
                {
                    if (await _skillsRepository.retrieveSkillAsync(skillId) != null)
                    {
                        await _skillsRepository.deleteSkillAsync(skillId);
                        result = NoContent();
                    }
                    else
                    {
                        result = NotFound($"The skill with {skillId} was not found");
                    }
                }
            }
            catch (Exception e)
            {
                // Internal Server error
                result = StatusCode(
                    statusCode: StatusCodes.Status500InternalServerError,
                    value: e.Message);
            }

            return result;
        }
    }
}