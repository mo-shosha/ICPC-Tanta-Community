using Core.DTO.memberDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.TeamDTO
{
    public class TeamWithMemberDto
    {
        public int Id { get; set; }
        public string TeamName { get; set; }

        public string Description { get; set; }

        public string? LogoURL { get; set; }

        public List<memberDto> Members { get; set; }
    }
}
