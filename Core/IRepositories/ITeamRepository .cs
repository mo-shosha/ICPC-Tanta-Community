using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface ITeamRepository: IBaseRepository<Team>
    {
        public Task<IEnumerable<Team>> AllTeamWithMember();
        public Task<IEnumerable<Team>> AllTeamWithMemberByYear(string year);
    }
}
