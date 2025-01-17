using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ScheduleDto
{
    public class ScheduleDtoo
    {
        public int Id { get; set; }
        public TimeSpan Time { get; set; }
        public string Activity { get; set; }
        public int EventId { get; set; }
    }
}
