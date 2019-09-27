using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenWebTech.Models
{
    public class Contact // : IdentityUser
    {        

        [Range(1, int.MaxValue, ErrorMessage = "The id must be greater than 0")]
        public int Id { get; set; }

        [Required(ErrorMessage = "The first name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The last name is required")]
        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public IList<Skill> Skills { get; internal set;  } = new List<Skill>();
    }
}
