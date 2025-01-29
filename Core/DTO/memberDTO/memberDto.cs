using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.memberDTO
{
    public class memberDto
    {
        public string FullName { get; set; }
        public string Role { get; set; }

        public string Name { get; set; }
        public string FacebookUrl { get; set; }
        public string LinkedInUrl { get; set; }

        public string ImgUrl { get; set; }


    }
}
