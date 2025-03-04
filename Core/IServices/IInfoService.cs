using Core.DTO.InfoDTO;
using ICPC_Tanta_Web.DTO.NewsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IInfoService
    {
        Task<InfoDto> GetInfAsync();
        Task AddAsync(CreateInfoDto createInfoDto);
        Task UpdateAsync(UpdateInfoDto updateInfoDto);
    }
}
