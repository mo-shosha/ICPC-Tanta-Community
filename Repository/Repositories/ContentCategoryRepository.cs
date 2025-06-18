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
    public class ContentCategoryRepository:BaseRepository<ContentCategory>, IContentCategoryRepository
    {
        public ContentCategoryRepository(AppDbContext db) : base(db)
        {
        }
    }
}
