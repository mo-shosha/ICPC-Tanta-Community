using Core.DTO.ContentDTO;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface ITrainingContentRepository:IBaseRepository<TrainingContent>
    {

        Task<TrainingContent> GetEntityWithLinksByIdAsync(int id);

        Task<ContentReadDto> GetContentWithLinksByIdAsync(int id);
        Task<IEnumerable<ContentReadDto>> GetContentWithLinksAsync();
    }
}
