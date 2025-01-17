using Core.DTO.EventDTO;
using Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICPC_Tanta_Web.DTO.NewsDTO;
using Core.Entities;

namespace Core.IServices
{
    public interface IEventService
    {
        Task<EventWithSchedulesDto> GetByIdAsync(int id);
        Task<IEnumerable<EventWithSchedulesDto>> GetAllAsync();
        //Task<EventDto> GetByIdAsync(int id);
        Task AddAsync(EventCreateDto createEventDto);
        Task UpdateAsync(EventUpdateDto updateEventDto);
        Task DeleteAsync(int id);
    }

}
