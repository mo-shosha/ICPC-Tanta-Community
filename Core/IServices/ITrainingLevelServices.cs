using Core.DTO.LevelDTO;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface ITrainingLevelServices
    {
        Task<IEnumerable<Leveldto>> GetAllLevels();        
        Task<Leveldto> GetLevelByIdAsync(int id);
        Task CreateLevelAsync(LevelCreateDto levelCreateDto);
        Task UpdateLevelAsync(LevelUpdateDto levelUpdateDto);
        Task DeleteLevelAsync(int id);
        Task<TrainingLevel> GetLevelsWithContentAsync(int id);
        Task<TrainingLevel> GetLevelWithContentByYearAsync(int id,string year);


    }
}
