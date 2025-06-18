using Core.Entities;
using Core.IServices;
using ICPC_Tanta_Web.Services;
using Repository.Data;

public class SocialMediaSyncService : ISocialMediaSyncService
{
    private readonly YoutubeService _youtubeService;
    private readonly AppDbContext _db;

    public SocialMediaSyncService(YoutubeService youtubeService, AppDbContext db)
    {
        _youtubeService = youtubeService;
        _db = db;
    }

    public async Task SyncYoutubeAsync()
    {
        var videos = await _youtubeService.GetLatestVideosAsync();

        var levels = _db.trainingLevels.ToList();
        var categories = _db.contentCategories.ToList();

        var newContents = videos
            .Where(v => !_db.trainingContents.Any(t => t.ContentUrl == v.VideoUrl))
            .Select(v =>
            {
                var levelName = ExtractLevelNameFromTitle(v.Title);  
                var level = levels.FirstOrDefault(l => l.Name.Contains(levelName, StringComparison.OrdinalIgnoreCase));

                var type = ExtractTypeFromTitle(v.Title);  
                var category = categories.FirstOrDefault(c => c.CategoryName.Contains(type, StringComparison.OrdinalIgnoreCase));

                return new TrainingContent
                {
                    Title = v.Title,
                    Auther = "ICPC Tanta",
                    CreatedAt = v.PublishedAt,
                    ContentUrl = v.VideoUrl,
                    TrainingLevelId = level?.Id ?? -1,
                    ContentCategoryId = category?.Id ?? -1
                };
            })
            .Where(c => c.TrainingLevelId != -1 && c.ContentCategoryId != -1)
            .ToList();

        if (newContents.Any())
        {
            await _db.trainingContents.AddRangeAsync(newContents);
            await _db.SaveChangesAsync();
        }
    }


    public Task SyncFacebookAsync()
    {
        return Task.CompletedTask;
    }

    private string ExtractLevelNameFromTitle(string title)
    {
        if (title.Contains("Level 0", StringComparison.OrdinalIgnoreCase)) return "Level 0";
        if (title.Contains("Level 1", StringComparison.OrdinalIgnoreCase)) return "Level 1";
        if (title.Contains("Level 2", StringComparison.OrdinalIgnoreCase)) return "Level 2";
        if (title.Contains("Level 3", StringComparison.OrdinalIgnoreCase)) return "Level 3";
        return "";
    }


    private string ExtractTypeFromTitle(string title)
    {
        if (title.Contains("Upsolve", StringComparison.OrdinalIgnoreCase)) return "Upsolve";
        else return "Explanation";
    }
}
