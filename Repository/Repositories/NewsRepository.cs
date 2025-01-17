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
    public class NewsRepository : BaseRepository<News>, INewsRepository
    {

        public NewsRepository(AppDbContext db) : base(db) { }

        public async Task<IEnumerable<News>> SearchAsync(string keyword)
        {
            return await _db.Set<News>()
                .Where(n => n.Title.Contains(keyword) || n.Description.Contains(keyword))
                .ToListAsync();
        }
    }
}
