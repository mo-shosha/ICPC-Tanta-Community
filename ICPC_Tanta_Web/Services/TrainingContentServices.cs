using Core.DTO.ContentDTO;
using Core.DTO.LevelDTO;
using Core.Entities;
using Core.IRepositories;
using Core.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

                var newContent = new TrainingContent()
                {
                    Title = contentCreateDto.Title,
                    CreatedAt = DateTime.Now,
                    ContentUrl = contentCreateDto.ContentUrl,
                    Auther = contentCreateDto.Auther,
                    TrainingLevelId = contentCreateDto.LevelId
                };

                await _unitOfWork.TrainingContentRepository.AddAsync(newContent);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the content.", ex);
            }
        }

        public async Task DeleteContentAsync(int id)
        {
            try
            {
                var content = await _unitOfWork.TrainingContentRepository.GetByIdAsync(id);
                if (content == null)
                {
                    throw new KeyNotFoundException("Content not found.");
                }

                _unitOfWork.TrainingContentRepository.Delete(content);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the content.", ex);
            }
        }

        public async Task<IEnumerable<TrainingContent>> GetAllContentAsync()
        {
            try
            {
                var contents = await _unitOfWork.TrainingContentRepository.GetAllAsync();
                return contents;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all content.", ex);
            }
        }

        public async Task<TrainingContent> GetContentAsyncById(int id)
        {
            try
            {
                var content = await _unitOfWork.TrainingContentRepository.GetByIdAsync(id);
                if (content == null)
                {
                    throw new KeyNotFoundException("Content not found.");
                }

                return content;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the content with ID {id}.", ex);
            }
        }

        public async Task UpdateContentAsync(ContentUpdateDto contentUpdateDto)
        {
            try
            {
                var existingContent = await _unitOfWork.TrainingContentRepository.GetByIdAsync(contentUpdateDto.Id);
                if (existingContent == null)
                {
                    throw new KeyNotFoundException("Content not found.");
                }

                // Update properties if they are not null
                existingContent.ContentUrl = contentUpdateDto.ContentUrl ?? existingContent.ContentUrl;
                existingContent.Title = contentUpdateDto.Title ?? existingContent.Title;
                existingContent.Auther = existingContent.Auther;

                _unitOfWork.TrainingContentRepository.Update(existingContent);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the content.", ex);
            }
        }
    }
}
