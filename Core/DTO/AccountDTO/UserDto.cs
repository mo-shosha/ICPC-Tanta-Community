using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Core.DTO.AccountDTO
{
    public class UserDto
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Handle { get; set; }
        public string Rank { get; set; }
        public int Rating { get; set; }
        public string TitlePhoto { get; set; }
        public List<string>? Roles { get; set; }
        public string? Token { get; set; }
        //public DateTime? ExpiresOn { get; set; }
        public string PhoneNumber { get; set; }
        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
