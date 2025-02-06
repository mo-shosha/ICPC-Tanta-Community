using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.AccountDTO
{
    public class UserRatingDto
    {
        public string Id {  get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public string ImgURL {  get; set; }
    }
}
