using Core.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.memberDTO
{
    public class memberUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string FullName { get; set; }
        public string? FacebookUrl { get; set; }
        public string? LinkedInUrl { get; set; }

        [Required]
        public string YearJoin { get; set; }

        [IsImage]
        public IFormFile? MemberImg { get; set; }

        [Required]
        public int TeamId { get; set; }
    }
}
