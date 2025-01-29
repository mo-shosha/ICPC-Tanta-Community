using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.memberDTO
{
    public class CreatMemberDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Role { get; set; }  // Role of the member (Head, Member)
        [Required]
        public string FacebookUrl { get; set; }
        [Required]
        public string LinkedInUrl { get; set; }
        [Required]
        public string YearJoin { get; set; }
        //[Required]//if girl not to share img
        public IFormFile? MemberImg { get; set; }

        public int TeamId {  get; set; }
    }
}
