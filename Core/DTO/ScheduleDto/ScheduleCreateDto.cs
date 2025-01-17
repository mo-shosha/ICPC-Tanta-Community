using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ScheduleDto
{
    public class ScheduleCreateDto
    {
        [Required]
        public TimeSpan Time { get; set; }

        [Required]
        public string Activity { get; set; }

        [Required]
        public int EventId { get; set; }
    }

}
