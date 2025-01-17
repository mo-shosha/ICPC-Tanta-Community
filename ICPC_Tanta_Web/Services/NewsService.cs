using Core.Entities;
using Core.IRepositories;
using Core.IServices;
using ICPC_Tanta_Web.DTO.NewsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICPC_Tanta_Web.Services
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileProcessingService _fileProcessingService;

        public NewsService(IUnitOfWork unitOfWork, IFileProcessingService fileProcessingService)
        {
            _unitOfWork = unitOfWork;
            _fileProcessingService = fileProcessingService;
        }

        public async Task<NewsDto> GetByIdAsync(int id)
        {
            var news = await _unitOfWork.NewsRepository.GetByIdAsync(id);
            return news != null ? MapToDto(news) : null;
        }

        public async Task<IEnumerable<NewsDto>> GetAllAsync()
        {
            var newsList = await _unitOfWork.NewsRepository.GetAllAsync();
            return newsList.Select(MapToDto);
        }

        public async Task AddAsync(CreateNewsDto createNewsDto)
        {
            var news = new News
            {
                Title = createNewsDto.Title,
                Description = createNewsDto.Description,
                Status = createNewsDto.Status,
                Author = createNewsDto.Author,
                CreatedDate = DateTime.Now
            };

            if (createNewsDto.Image != null)
            {
                news.ImageUrl = await _fileProcessingService.SaveFileAsync(createNewsDto.Image);
            }

            await _unitOfWork.NewsRepository.AddAsync(news);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateNewsDto updateNewsDto)
        {
            var news = await _unitOfWork.NewsRepository.GetByIdAsync(updateNewsDto.Id);
            if (news == null) throw new KeyNotFoundException("News not found");

            news.Title = updateNewsDto.Title;
            news.Description = updateNewsDto.Description;
            news.Status = updateNewsDto.Status;
            news.Author = updateNewsDto.Author;

            if (updateNewsDto.Image != null)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(news.ImageUrl))
                {
                    _fileProcessingService.DeleteFile(news.ImageUrl);
                }

                // Save new image
                news.ImageUrl = await _fileProcessingService.SaveFileAsync(updateNewsDto.Image);
            }

            _unitOfWork.NewsRepository.Update(news);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var news = await _unitOfWork.NewsRepository.GetByIdAsync(id);
            if (news == null) throw new KeyNotFoundException("News not found");

            // Delete image if exists
            if (!string.IsNullOrEmpty(news.ImageUrl))
            {
                _fileProcessingService.DeleteFile(news.ImageUrl);
            }

            _unitOfWork.NewsRepository.Delete(news);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<NewsDto>> SearchAsync(string keyword)
        {
            var results = await _unitOfWork.NewsRepository.SearchAsync(keyword);
            return results.Select(MapToDto);
        }

        private NewsDto MapToDto(News news)
        {
            return new NewsDto
            {
                Id = news.Id,
                Title = news.Title,
                Description = news.Description,
                Status = news.Status,
                Author = news.Author,
                ImageUrl = news.ImageUrl,
                CreatedDate = news.CreatedDate,
                PublishedDate = news.PublishedDate
            };
        }
    }
}
