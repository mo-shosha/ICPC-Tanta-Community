using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.AccountDTO
{
    public class ResetPasswordDto
    {
        [Required] 
        public string Email { get; set; }
        public string Token { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
