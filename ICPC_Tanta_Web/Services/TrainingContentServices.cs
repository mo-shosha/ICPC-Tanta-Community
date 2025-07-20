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


                var level = await _unitOfWork.TrainingLevelRepository.GetByIdAsync(contentCreateDto.LevelId);
                if (level == null)
                {
                    throw new KeyNotFoundException($"Training level with ID {contentCreateDto.LevelId} not found.");
                }


                //var category = await _unitOfWork.ContentCategoryRepository.GetByIdAsync(contentCreateDto.CategoryId);
                //if (category == null)
                //{
                //    throw new KeyNotFoundException($"Content category with ID {contentCreateDto.CategoryId} not found.");
                //}
                var anotherLinks = contentCreateDto.AnotherLinks?
                    .Select(dto => new AnotherLink
                    {
                        Title = dto.Title,
                        Url = dto.Url
                    }).ToList();

                var newContent = new TrainingContent()
                {
                    Title = contentCreateDto.Title,
                    WeekNumber=contentCreateDto.WeekNumber,
                    ExplanationLink = contentCreateDto.ExplanationLink,
                    ExplanationBy=contentCreateDto.ExplanationBy,
                    UpsolveLink=contentCreateDto.UpsolveLink,
                    UpsolveBy=contentCreateDto.UpsolveBy,
                    SheetLink=contentCreateDto.SheetLink,
                    AnotherLinks=anotherLinks,
                    TrainingLevelId = contentCreateDto.LevelId,
                    //CreatedAt=DateTime.Now
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

        public async Task<IEnumerable<ContentReadDto>> GetAllContentAsync()
        {
            try
            {
                var contents = await _unitOfWork.TrainingContentRepository.GetContentWithLinksAsync();
                return contents;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all content.", ex);
            }
        }

        public async Task<ContentReadDto> GetContentAsyncById(int id)
        {
            try
            {
                var content = await _unitOfWork.TrainingContentRepository.GetContentWithLinksByIdAsync(id);
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
                var existingContent = await _unitOfWork.TrainingContentRepository
                    .GetEntityWithLinksByIdAsync(contentUpdateDto.Id); 

                if (existingContent == null)
                {
                    throw new KeyNotFoundException("Content not found.");
                }

                existingContent.Title = contentUpdateDto.Title;
                existingContent.WeekNumber = contentUpdateDto.WeekNumber;
                existingContent.ExplanationLink = contentUpdateDto.ExplanationLink;
                existingContent.ExplanationBy = contentUpdateDto.ExplanationBy;
                existingContent.UpsolveLink = contentUpdateDto.UpsolveLink;
                existingContent.UpsolveBy = contentUpdateDto.UpsolveBy;
                existingContent.SheetLink = contentUpdateDto.SheetLink;

                if (contentUpdateDto.AnotherLinks != null)
                {
                    existingContent.AnotherLinks?.Clear();

                    existingContent.AnotherLinks = contentUpdateDto.AnotherLinks
                        .Select(dto => new AnotherLink
                        {
                            Title = dto.Title,
                            Url = dto.Url
                        }).ToList();
                }

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
