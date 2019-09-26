using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenWebTech.Models
{
    public class Skill
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public string Level { get; set; }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Skill s = (Skill)obj;
                return (Id == s.Id);
            }
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
