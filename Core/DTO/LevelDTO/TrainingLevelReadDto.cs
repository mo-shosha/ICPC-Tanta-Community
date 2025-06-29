using Core.DTO.ContentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.LevelDTO
{
    public class TrainingLevelReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string LevelImg { get; set; }

        public List<ContentReadDto> Contents { get; set; } = new();  
    }
}
