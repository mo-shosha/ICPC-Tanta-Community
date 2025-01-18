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
    public class TeamRepository: BaseRepository<Team>, ITeamRepository
    {
        public TeamRepository(AppDbContext db) : base(db)
        {
        }
    }
}
