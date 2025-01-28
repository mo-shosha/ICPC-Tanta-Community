using Core.Entities;
using Core.IRepositories;
using Core.IServices;
using ICPC_Tanta_Web.DTO.NewsDTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IWebHostEnvironment _webHostEnvironment;

        public NewsService(IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
           // _webHostEnvironment = webHostEnvironment;
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
                //Status = createNewsDto.Status,
                Author = createNewsDto.Author,
                CreatedDate = DateTime.Now
            };

            if (createNewsDto.Image != null)
            {
                string fileName = SaveImage(createNewsDto.Image);
                news.ImageUrl = $"/uploads/{fileName}";
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

            if (updateNewsDto.Image != null)
            {
                string fileName = SaveImage(updateNewsDto.Image);
                news.ImageUrl = $"/uploads/{fileName}";
            }

            _unitOfWork.NewsRepository.Update(news);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var news = await _unitOfWork.NewsRepository.GetByIdAsync(id);
            if (news == null) throw new KeyNotFoundException("News not found");

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
                Author = news.Author,
                ImageUrl = news.ImageUrl,
                CreatedDate = news.CreatedDate,
                PublishedDate = news.PublishedDate
            };
        }

        private string SaveImage(IFormFile image)
        {
            return "";
        }
        //{
        //    string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
        //    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        //    string fileName = $"{Guid.NewGuid()}_{image.FileName}";
        //    string filePath = Path.Combine(folderPath, fileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        image.CopyTo(stream);
        //    }

        //    return fileName;
        //}
    }
}
