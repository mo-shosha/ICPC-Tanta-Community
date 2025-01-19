using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.TeamDTO
{
    public class TeamDTO
    {
        public int Id { get; set; } 
        public string TeamName { get; set; }
        public string Description { get; set; }
        public string LogoURL { get; set; }
       // public List<Member> Members { get; set; }
    }
}
