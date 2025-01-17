using ICPC_Tanta_Web.DTO.NewsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface INewsService
    {
        Task<NewsDto> GetByIdAsync(int id);
        Task<IEnumerable<NewsDto>> GetAllAsync();
        Task AddAsync(CreateNewsDto createNewsDto);
        Task UpdateAsync(UpdateNewsDto updateNewsDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<NewsDto>> SearchAsync(string keyword);
    }
}
