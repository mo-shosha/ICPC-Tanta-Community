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
    public class InfoRepository : BaseRepository<info>, IInfoRepository
    {
        public InfoRepository(AppDbContext db) : base(db)
        {
        }
    }
}
