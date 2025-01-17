using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Team
    {
        public int Id { get; set; }  

        [Required]
        public string TeamName { get; set; }   

        public string Description { get; set; }  

        public string LogoURL { get; set; } 

        public List<Member> Members { get; set; }
    }
}
