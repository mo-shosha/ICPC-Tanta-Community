using Core.DTO.ContentDTO;
using Core.DTO.LevelDTO;
using Core.Entities;
using Core.IRepositories;
using Core.IServices;
using System.Numerics;
using static MediaBrowser.Common.Updates.GithubUpdater;

namespace ICPC_Tanta_Web.Services
{
    public class TrainingContentServices : ITrainingContentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public TrainingContentServices(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }
        public async Task CreateContentAsync(ContentCreateDto contentCreateDto)
        {
            try
            {
                if (string.IsNullOrEmpty(contentCreateDto.Title))
                {
                    throw new ArgumentException("Content Title cannot be empty.");
                }
                
                var newcontent = new TrainingContent()
                {
                    Title = contentCreateDto.Title,
                    CreatedAt = DateTime.Now,
                    ContentUrl = contentCreateDto.ContentUrl,
                    Auther=contentCreateDto.Auther,
                    TrainingLevelId=contentCreateDto.LevelId
                };
                await _unitOfWork.TrainingContentRepository.AddAsync(newcontent);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while creating the content.");
            }
        }
    }
}
