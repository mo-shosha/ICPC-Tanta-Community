using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IScheduleRepository:IBaseRepository<Schedule>
    {
        Task<IEnumerable<Schedule>> GetSchedulesByEventIdAsync(int eventId);
    }

}
