using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Validation;

namespace Core.DTO.EventDTO
{
    public class EventCreateDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public string TicketUrl { get; set; }

        [IsImage]
        public IFormFile Image { get; set; }
    }
}
