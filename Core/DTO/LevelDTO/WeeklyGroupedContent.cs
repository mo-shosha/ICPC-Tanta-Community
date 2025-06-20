using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.LevelDTO
{
    public class WeeklyGroupedContent
    {
        public int? WeekNumber { get; set; }  
        public TrainingContent Explanation { get; set; }
        public TrainingContent Sheet { get; set; }
        public TrainingContent Upsolve { get; set; }
    }

}
