using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Events
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        public string TicketUrl {  get; set; }

        public string ImgUrl { get; set; }
        public List<Schedule> DailyPlan { get; set; }

    }
}
