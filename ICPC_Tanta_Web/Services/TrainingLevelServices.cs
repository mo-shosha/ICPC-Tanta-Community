using Core.DTO.LevelDTO;
using Core.Entities;
using Core.IRepositories;
using Core.IServices;
using ICPC_Tanta_Web.DTO.NewsDTO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ICPC_Tanta_Web.Services
{
    public class TrainingLevelServices: ITrainingLevelServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileProcessingService _fileProcessingService;
        public TrainingLevelServices(IUnitOfWork unitOfWork,IFileProcessingService fileProcessingService)
        {
            _unitOfWork = unitOfWork;
            _fileProcessingService = fileProcessingService;
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
                    //LevelImg = level.LevelImg,
                    //LevelDescription = level.Description
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
                    //LevelImg=level.LevelImg,
                    //LevelDescription=level.Description
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
                    Description = levelCreateDto.Description,
                    LevelImg = levelCreateDto.Image != null
                        ? await _fileProcessingService.SaveFileAsync(levelCreateDto.Image)
                        : null
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
                if (levelUpdateDto.Image != null)
                {
                    if (existingLevel.LevelImg != null)
                    {
                        _fileProcessingService.DeleteFile(existingLevel.LevelImg);
                    }
                    existingLevel.LevelImg = await _fileProcessingService.SaveFileAsync(levelUpdateDto.Image);
                }
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


        public async Task<TrainingLevelWithWeeklyContent> GetLevelsWithContentAsync(int id)
        {
            try
            {
                var levels = await _unitOfWork.TrainingLevelRepository.LevelWithContentsAync();
                var selectedLevel = levels.FirstOrDefault(l => l.Id == id);

                if (selectedLevel == null)
                    return null;

                var grouped = GroupContentsByWeek(selectedLevel.Contents);

                return new TrainingLevelWithWeeklyContent
                {
                    Id = selectedLevel.Id,
                    Name = selectedLevel.Name,
                    Description = selectedLevel.Description,
                    LevelImg = selectedLevel.LevelImg,
                    WeeklyContents = grouped
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving levels with content.", ex);
            }
        }


        public async Task<TrainingLevelWithWeeklyContent> GetLevelWithContentByYearAsync(int id, string year)
        {
            try
            {
                var levels = await _unitOfWork.TrainingLevelRepository.LevelWithContentByYearAync(year);
                var selectedLevel = levels.FirstOrDefault(l => l.Id == id);

                if (selectedLevel == null)
                    return null;

                var grouped = GroupContentsByWeek(selectedLevel.Contents);

                return new TrainingLevelWithWeeklyContent
                {
                    Id = selectedLevel.Id,
                    Name = selectedLevel.Name,
                    Description = selectedLevel.Description,
                    LevelImg = selectedLevel.LevelImg,
                    WeeklyContents = grouped
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving levels for the year {year}.", ex);
            }
        }

        private List<WeeklyGroupedContent> GroupContentsByWeek(IEnumerable<TrainingContent> contents)
        {
            string GetType(TrainingContent c)
            {
                var name = c.ContentCategory?.CategoryName?.ToLower() ?? "";
                if (name.Contains("explanation")) return "explanation";
                if (name.Contains("sheet")) return "sheet";
                if (name.Contains("upsolve")) return "upsolve";
                return "unknown";
            }

            int ExtractWeek(string title)
            {
                var match = Regex.Match(title.ToLower(), @"week\s*(\d+)");
                return match.Success ? int.Parse(match.Groups[1].Value) : -1;
            }

            return contents
                .GroupBy(c => ExtractWeek(c.Title))
                .Where(g => g.Key != -1)
                .Select(g => new WeeklyGroupedContent
                {
                    WeekNumber = g.Key,
                    Explanation = ProjectContentWithoutLevel(g.FirstOrDefault(c => GetType(c) == "explanation")),
                    Sheet = ProjectContentWithoutLevel(g.FirstOrDefault(c => GetType(c) == "sheet")),
                    Upsolve = ProjectContentWithoutLevel(g.FirstOrDefault(c => GetType(c) == "upsolve"))
                })
                .OrderBy(w => w.WeekNumber)
                .ToList();
        }

        private TrainingContent ProjectContentWithoutLevel(TrainingContent c)
        {
            if (c == null) return null;

            return new TrainingContent
            {
                Id = c.Id,
                Title = c.Title,
                ContentUrl = c.ContentUrl,
                Auther = c.Auther,
                CreatedAt = c.CreatedAt,
                ContentCategoryId = c.ContentCategoryId,
                ContentCategory = c.ContentCategory != null
                    ? new ContentCategory
                    {
                        Id = c.ContentCategory.Id,
                        CategoryName = c.ContentCategory.CategoryName
                    }
                    : null,
            };
        }


    }
}
