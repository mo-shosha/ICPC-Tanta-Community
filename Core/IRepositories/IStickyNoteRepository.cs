using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IStickyNoteRepository:IBaseRepository<StickyNotes>
    {
        Task<IEnumerable<StickyNotes>> GetLastAsync();
    }
}
