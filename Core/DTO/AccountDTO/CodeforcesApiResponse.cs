using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.AccountDTO
{
    public class CodeforcesApiResponse
    {
        public string Status { get; set; }
        public List<CodeforcesUserInfo> Result { get; set; }
    }
}
