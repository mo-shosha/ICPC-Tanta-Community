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

        var newContents = videos
            .Where(v => !_db.trainingContents.Any(t => t.ContentUrl == v.VideoUrl))
            .Select(v => new TrainingContent
            {
                Title = v.Title,
                Auther = "ICPC Tanta",
                CreatedAt = v.PublishedAt,
                ContentUrl = v.VideoUrl,
                TrainingLevelId = ExtractLevelFromTitle(v.Title)
            })
            .Where(c => c.TrainingLevelId != -1)
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

    private int ExtractLevelFromTitle(string title)
    {
        if (title.Contains("Level 0", StringComparison.OrdinalIgnoreCase)) return 1;
        if (title.Contains("Level 1", StringComparison.OrdinalIgnoreCase)) return 2;
        if (title.Contains("Level 2", StringComparison.OrdinalIgnoreCase)) return 3;
        if (title.Contains("Level 3", StringComparison.OrdinalIgnoreCase)) return 4;
        return -1;
    }
}
