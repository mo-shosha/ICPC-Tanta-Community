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
    public class StickyNoteRepository : BaseRepository<StickyNotes>, IStickyNoteRepository
    {
        private readonly AppDbContext _context;
        public StickyNoteRepository(AppDbContext db) : base(db)
        {
            _context=db;
        }

        public async Task<IEnumerable<StickyNotes>> GetLastAsync()
        {
            int count = 50;
            return await _context.StickyNotes
                .OrderByDescending(s => s.Id)
                .Take(count)
                .ToListAsync();
        }
    }
}
