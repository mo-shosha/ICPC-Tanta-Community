using Core.DTO.ScheduleDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IScheduleService
    {
        Task<IEnumerable<ScheduleDtoo>> GetSchedulesByEventIdAsync(int eventId);
        Task AddAsync(ScheduleCreateDto createScheduleDto);
        Task UpdateAsync(ScheduleUpdateDto updateScheduleDto);
        Task DeleteAsync(int id);
    }
}
