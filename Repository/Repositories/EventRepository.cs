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
    public class EventRepository : BaseRepository<Events>, IEventRepository
    {
        public EventRepository(AppDbContext db) : base(db)
        {
        }

        public async Task<Events> GetEventByTitleAsync(string title)
        {
            return await _db.Set<Events>()
                   .Where(e => e.Title == title)
                   .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Events>> GetEventsByDateAsync(DateTime date)
        {
            return await _db.Set<Events>()
                   .Where(e => e.DateTime.Date == date.Date)
                   .ToListAsync();
        }

        public async Task<IEnumerable<Events>> GetEventsWithSchedulesAsync()
        {
            return await _db.Set<Events>()
                .Include(e => e.DailyPlan)
                .ToListAsync();
        }

        public  async Task<Events> GetEventWithSchedulesAsyncById(int id)
        {
            return await _db.Set<Events>()
               .Include(e => e.DailyPlan).Where(e => e.Id == id).FirstOrDefaultAsync();
        }
    }
}
