using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.QAndADTO
{
    public class QAndAReadDto
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string AnsweredBy { get; set; }
        public DateTime AnsweredAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
