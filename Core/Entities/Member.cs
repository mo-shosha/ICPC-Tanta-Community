using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Member
    {
        public int Id { get; set; }   
        [Required]
        public string FullName { get; set; }   
        [Required]
        public string Role { get; set; }  // Role of the member (Head, Member)
        [Required]
        public string Email { get; set; } 
        [Required]
        public string FacebookUrl { get; set; }
        [Required]
        public string LinkedInUrl { get; set; }
        public DateTime YearJoin { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
