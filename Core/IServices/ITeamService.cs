using Core.DTO.TeamDTO;
using ICPC_Tanta_Web.DTO.NewsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface ITeamService
    {
        Task<TeamDTO> GetByIdAsync(int id);
        Task<IEnumerable<TeamDTO>> GetAllAsync();
        Task AddAsync(CreateTeamDTO createNewsDto);
        Task UpdateAsync(UpdateTeamDto updateNewsDto);
        Task DeleteAsync(int id);
    }
}
