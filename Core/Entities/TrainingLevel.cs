﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class TrainingLevel
    {
        public int Id { get; set; }   

        [Required]
        public string Name { get; set; }  
        public string Description { get; set; }
        public string LevelImg {  get; set; }
        public List<TrainingContent> Contents { get; set; }
    }
}
