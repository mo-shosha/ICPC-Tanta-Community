using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.ScheduleDto;
namespace Core.DTO
{
    public class EventWithSchedulesDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
        public string TicketUrl { get; set; }
        public string ImgUrl { get; set; }
        public List<ScheduleDtoo> DailyPlan { get; set; }
    }
}
