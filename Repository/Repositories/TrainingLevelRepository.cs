using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
namespace Repository.Repositories
{
    public class TrainingLevelRepository : BaseRepository<TrainingLevel>, ITrainingLevelRepository
    {
        public TrainingLevelRepository(AppDbContext db) : base(db)
        {
        }

        

        public async Task<IEnumerable<TrainingLevel>> LevelWithContentByYearAync(int id, string year)
        {
            return await _db.Set<TrainingLevel>()
             .Include(t => t.Contents)
             .Where(t => t.Id == id)
             .Where(t => t.Contents.Any(m => m.CreatedAt.Year.ToString() == year))
             .ToListAsync();   
        }

        public async Task<IEnumerable<TrainingLevel>> LevelWithContentsAync(int id)
        {
            return await _db.Set<TrainingLevel>()
               .Include(t => t.Contents)
               .Where(t=>t.Id==id)
               .ToListAsync();
        }
    }
}
