using Core.DTO.ContentDTO;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface ITrainingContentServices
    {
        Task<IEnumerable<TrainingContent>> GetAllContentAsync();
        Task<TrainingContent> GetContentAsyncById(int id);
        Task CreateContentAsync(ContentCreateDto contentCreateDto);
        Task UpdateContentAsync(ContentUpdateDto contentUpdateDto);
        Task DeleteContentAsync(int id);

    }
}
