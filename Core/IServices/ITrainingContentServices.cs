using Core.DTO.ContentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface ITrainingContentServices
    {
        Task CreateContentAsync(ContentCreateDto contentCreateDto);
    }
}
