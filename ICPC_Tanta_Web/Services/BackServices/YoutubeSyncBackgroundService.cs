//using Core.IServices;

//namespace ICPC_Tanta_Web.Services.BackServices;
//public class YoutubeSyncBackgroundService : BackgroundService
//{
//    private readonly IServiceProvider _serviceProvider;
//    private readonly ILogger<YoutubeSyncBackgroundService> _logger;
//    private readonly TimeSpan _interval = TimeSpan.FromDays(7); 

//    public YoutubeSyncBackgroundService(IServiceProvider serviceProvider, ILogger<YoutubeSyncBackgroundService> logger)
//    {
//        _serviceProvider = serviceProvider;
//        _logger = logger;
//    }

//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        while (!stoppingToken.IsCancellationRequested)
//        {
//            try
//            {
//                using (var scope = _serviceProvider.CreateScope())
//                {
//                    var syncService = scope.ServiceProvider.GetRequiredService<ISocialMediaSyncService>();
//                    await syncService.SyncYoutubeAsync();
//                }

//                _logger.LogInformation("YouTube sync completed at: {time}", DateTimeOffset.Now);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error during YouTube sync.");
//            }

//            await Task.Delay(_interval, stoppingToken);  
//        }
//    }
//}
