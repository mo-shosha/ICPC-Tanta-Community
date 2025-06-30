using Core.Entities;
using Core.IRepositories;
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
        public StickyNoteRepository(AppDbContext db) : base(db)
        {
        }

    }
}
