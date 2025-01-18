using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IUnitOfWork:IDisposable
    {
        INewsRepository NewsRepository { get; }
        IEventRepository EventRepository { get; }
        IScheduleRepository ScheduleRepository { get; }

        ITeamRepository TeamRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
