using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.AccountDTO
{
    public class Userinfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Handle { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsLocked { get; set; }
    }
    public class CustemUserInfo: Userinfo
    {
        public string Rank { get; set; }
        public int Rating { get; set; }
        public List<string>? Roles { get; set; }
    }
}
