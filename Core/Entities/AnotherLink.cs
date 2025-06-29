using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class AnotherLink
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Url { get; set; }

        public int TrainingContentId { get; set; }
        public TrainingContent TrainingContent { get; set; }
    }
}
