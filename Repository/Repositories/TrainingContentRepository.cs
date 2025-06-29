using Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Repository.Data;
using Microsoft.EntityFrameworkCore;
using Core.DTO.ContentDTO;
namespace Repository.Repositories
{
    public class TrainingContentRepository: BaseRepository<TrainingContent>, ITrainingContentRepository
    {
        public TrainingContentRepository(AppDbContext context) : base(context) { }

        public async Task<TrainingContent> GetEntityWithLinksByIdAsync(int id)
        {
            return await _db.trainingContents
                .Include(tc => tc.AnotherLinks)
                .FirstOrDefaultAsync(tc => tc.Id == id);
        }

        public async Task<ContentReadDto> GetContentWithLinksByIdAsync(int id)
        {
            return await _db.trainingContents
                .Include(tc => tc.AnotherLinks)
                .Where(tc => tc.Id == id)
                .Select(tc => new ContentReadDto
                {
                    Id = tc.Id,
                    Title = tc.Title,
                    WeekNumber = tc.WeekNumber,
                    ExplanationLink = tc.ExplanationLink,
                    ExplanationBy = tc.ExplanationBy,
                    UpsolveLink = tc.UpsolveLink,
                    UpsolveBy = tc.UpsolveBy,
                    SheetLink = tc.SheetLink,
                    AnotherLinks = tc.AnotherLinks.Select(link => new AnotherLinkDto
                    {
                        Title = link.Title,
                        Url = link.Url
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ContentReadDto>> GetContentWithLinksAsync()
        {
            return await _db.trainingContents
                .Include(tc => tc.AnotherLinks)
                .Select(tc => new ContentReadDto
                {
                    Id = tc.Id,
                    Title = tc.Title,
                    WeekNumber = tc.WeekNumber,
                    ExplanationLink = tc.ExplanationLink,
                    ExplanationBy = tc.ExplanationBy,
                    UpsolveLink = tc.UpsolveLink,
                    UpsolveBy = tc.UpsolveBy,
                    SheetLink = tc.SheetLink,
                    AnotherLinks = tc.AnotherLinks.Select(link => new AnotherLinkDto
                    {
                        Title = link.Title,
                        Url = link.Url
                    }).ToList()
                })
                .ToListAsync();
        }

    }
}

