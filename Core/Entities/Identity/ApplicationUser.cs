using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string CodeForcesHandel {  get; set; }

        public string FullName {  get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; }

    }
}
