using Core.Entities;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using Repository.Data;


namespace Repository.Repositories
{
    class QAndARepository : BaseRepository<QAndA>, IQAndARepository
    {
        private readonly AppDbContext _context;
        public QAndARepository(AppDbContext db) : base(db)
        {
            _context = db;
        }
        public async Task<IEnumerable<QAndA>> GetLastAsync()
        {
            int count = 50;
            return await _context.qAndAs
                .Where(q => !string.IsNullOrWhiteSpace(q.Answer))
                .OrderByDescending(s => s.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

    }
}
