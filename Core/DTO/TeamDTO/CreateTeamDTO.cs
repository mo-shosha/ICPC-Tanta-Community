using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.TeamDTO
{
    public class CreateTeamDTO
    {
        [Required]
        public string TeamName { get; set; }
        public string Description { get; set; }
        public IFormFile? LogoImg { get; set; }
    }
}
