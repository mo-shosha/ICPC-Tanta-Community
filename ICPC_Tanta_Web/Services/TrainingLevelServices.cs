using Core.DTO.LevelDTO;
using Core.Entities;
using Core.IRepositories;
using Core.IServices;

namespace ICPC_Tanta_Web.Services
{
    public class TrainingLevelServices: ITrainingLevelServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public TrainingLevelServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Leveldto>> GetAllLevels()
        {
            try
            {
                var levels = await _unitOfWork.TrainingLevelRepository.GetAllAsync();
                return levels.Select(level => new Leveldto
                {
                    Id = level.Id,
                    LevelName = level.Name,
                }).ToList();
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while retrieving all levels.");
            }
        }

        public async Task<Leveldto> GetLevelByIdAsync(int id)
        {
            try
            {
                var level = await _unitOfWork.TrainingLevelRepository.GetByIdAsync(id);
                if (level == null)
                {
                    throw new KeyNotFoundException("Level not found.");
                }

                return new Leveldto
                {
                    Id = level.Id,
                    LevelName = level.Name,                    
                };
            }
            catch (Exception)
            {
                throw new Exception($"An error occurred while retrieving the level with ID {id}.");
            }
        }

        public async Task CreateLevelAsync(LevelCreateDto levelCreateDto)
        {
            try
            {
                if (string.IsNullOrEmpty(levelCreateDto.LevelName))
                {
                    throw new ArgumentException("Level name cannot be empty.");
                }

                var newLevel = new TrainingLevel
                {
                    Name = levelCreateDto.LevelName,
                    Description = levelCreateDto.Description
                };
                

                await _unitOfWork.TrainingLevelRepository.AddAsync(newLevel);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while creating the level.");
            }
        }

        public async Task UpdateLevelAsync(LevelUpdateDto levelUpdateDto)
        {
            try
            {
                var existingLevel = await _unitOfWork.TrainingLevelRepository.GetByIdAsync(levelUpdateDto.Id);
                if (existingLevel == null)
                {
                    throw new KeyNotFoundException("Level not found.");
                }

                existingLevel.Name = levelUpdateDto.LevelName ?? existingLevel.Name;
                existingLevel.Description = levelUpdateDto.Description ?? existingLevel.Description;

                _unitOfWork.TrainingLevelRepository.Update(existingLevel);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while updating the level.");
            }
        }

        public async Task DeleteLevelAsync(int id)
        {
            try
            {
                var level = await _unitOfWork.TrainingLevelRepository.GetByIdAsync(id);
                if (level == null)
                {
                    throw new KeyNotFoundException("Level not found.");
                }

                _unitOfWork.TrainingLevelRepository.Delete(level);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception($"An error occurred while deleting the level with ID {id}.");
            }
        }

        public async Task<IEnumerable<TrainingLevel>> GetLevelsWithContentAsync(int id )
        {
            try
            {
                return await _unitOfWork.TrainingLevelRepository.LevelWithContentsAync(id);
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while retrieving levels with content.");
            }
        }

        public async Task<IEnumerable<TrainingLevel>> GetLevelWithContentByYearAsync(int id,string year)
        {
            try
            {

                return await _unitOfWork.TrainingLevelRepository.LevelWithContentByYearAync(id,year);
            }
            catch (Exception)
            {
                throw new Exception($"An error occurred while retrieving levels for the year {year}.");
            }
        }

         
    }
}
