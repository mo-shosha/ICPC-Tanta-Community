using Core.Entities;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class ScheduleRepository :BaseRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(AppDbContext db) : base(db)
        {
        }
        public async Task<IEnumerable<Schedule>> GetSchedulesByEventIdAsync(int eventId)
        {
            return await _db.Set<Schedule>().Where(s => s.EventId == eventId).ToListAsync();
        }
    }
}
