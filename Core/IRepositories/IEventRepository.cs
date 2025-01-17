using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IEventRepository:IBaseRepository<Events>
    {
        Task<Events> GetEventByTitleAsync(string title);
        Task<IEnumerable<Events>> GetEventsWithSchedulesAsync();

        Task<Events> GetEventWithSchedulesAsyncById(int id);
        Task<IEnumerable<Events>> GetEventsByDateAsync(DateTime date);
        //Task<IEnumerable<Events>> SearchAsync(string keyword);
    }
}
