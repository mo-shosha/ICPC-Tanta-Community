using Core.Entities;
using Core.IRepositories;
using Core.IServices;
using ICPC_Tanta_Web.DTO.NewsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ICPC_Tanta_Web.Services
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileProcessingService _fileProcessingService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NewsService(IUnitOfWork unitOfWork, IFileProcessingService fileProcessingService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _fileProcessingService = fileProcessingService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<NewsDto> GetByIdAsync(int id)
        {
            try
            {
                var news = await _unitOfWork.NewsRepository.GetByIdAsync(id);
                return news != null ? MapToDto(news) : null;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching news with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<NewsDto>> GetAllAsync()
        {
            try
            {
                var newsList = await _unitOfWork.NewsRepository.GetAllAsync();
                return newsList?.Select(MapToDto) ?? new List<NewsDto>();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all news.", ex);
            }
        }

        public async Task AddAsync(CreateNewsDto createNewsDto, string author, string authorId)
        {
            try
            {
                if (string.IsNullOrEmpty(author))
                    throw new UnauthorizedAccessException("User is not authenticated");

                var news = new News
                {
                    Title = createNewsDto.Title,
                    Description = createNewsDto.Description,
                    Author = author,
                    AutherId = authorId,
                    CreatedDate = DateTime.Now,
                    ImageUrl = createNewsDto.Image != null
                        ? await _fileProcessingService.SaveFileAsync(createNewsDto.Image)
                        : null
                };

                await _unitOfWork.NewsRepository.AddAsync(news);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException("Unauthorized: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding news.", ex);
            }
        }

        public async Task UpdateAsync(UpdateNewsDto updateNewsDto)
        {
            try
            {
                var news = await _unitOfWork.NewsRepository.GetByIdAsync(updateNewsDto.Id);
                if (news == null)
                    throw new KeyNotFoundException("News not found");

                news.Title = updateNewsDto.Title;
                news.Description = updateNewsDto.Description;

                if (updateNewsDto.Image != null)
                {
                    _fileProcessingService.DeleteFile(news.ImageUrl);
                    news.ImageUrl = await _fileProcessingService.SaveFileAsync(updateNewsDto.Image);
                }

                _unitOfWork.NewsRepository.Update(news);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException("News not found: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating news.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var news = await _unitOfWork.NewsRepository.GetByIdAsync(id);
                if (news == null)
                    throw new KeyNotFoundException("News not found");

                if (!string.IsNullOrEmpty(news.ImageUrl))
                    _fileProcessingService.DeleteFile(news.ImageUrl);

                _unitOfWork.NewsRepository.Delete(news);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException("News not found: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting news.", ex);
            }
        }

        public async Task<IEnumerable<NewsDto>> SearchAsync(string keyword)
        {
            try
            {
                var results = await _unitOfWork.NewsRepository.SearchAsync(keyword);
                return results?.Select(MapToDto) ?? Enumerable.Empty<NewsDto>();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while searching for news.", ex);
            }
        }

        private NewsDto MapToDto(News news)
        {
            try
            {
                return new NewsDto
                {
                    Id = news.Id,
                    Title = news.Title,
                    Description = news.Description,
                    Author = news.Author,
                    AutherId = news.AutherId,
                    ImageUrl = news.ImageUrl,
                    CreatedDate = news.CreatedDate,
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while mapping news data.", ex);
            }
        }
    }
}
