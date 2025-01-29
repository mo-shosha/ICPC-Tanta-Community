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
    public class TeamRepository: BaseRepository<Team>, ITeamRepository
    {
        public TeamRepository(AppDbContext db) : base(db)
        {
        }

        public async Task<IEnumerable<Team>> AllTeamWithMember()
        {
            return await _db.Set<Team>()
                .Include(t => t.Members)
                .ToListAsync();
        }

        public async Task<IEnumerable<Team>> AllTeamWithMemberByYear(string year)
        {
            return await _db.Set<Team>()
                .Include(t => t.Members)
                .Where(t => t.Members.Any(m => m.YearJoin == year)) 
                .ToListAsync();

        }
    }
}
