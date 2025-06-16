using Core.helper;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Options;

namespace ICPC_Tanta_Web.Services
{
    public class YoutubeService
    {
        private readonly YouTubeSettings _settings;

        public YoutubeService(IOptions<YouTubeSettings> options)
        {
            _settings = options.Value;
        }

        public async Task<List<YoutubeVideoDto>> GetLatestVideosAsync(int maxResults = 10)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _settings.ApiKey
            });

            var request = youtubeService.Search.List("snippet");
            request.ChannelId = _settings.ChannelId;
            request.MaxResults = maxResults;
            request.Order = SearchResource.ListRequest.OrderEnum.Date;

            var response = await request.ExecuteAsync();

            return response.Items
                .Where(item => item.Id.Kind == "youtube#video")
                .Select(item => new YoutubeVideoDto
                {
                    Title = item.Snippet.Title,
                    VideoUrl = $"https://www.youtube.com/watch?v={item.Id.VideoId}",
                    ThumbnailUrl = item.Snippet.Thumbnails?.High?.Url,
                    PublishedAt = item.Snippet.PublishedAt ?? DateTime.UtcNow
                })
                .ToList();
        }
    }

    public class YoutubeVideoDto
    {
        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
