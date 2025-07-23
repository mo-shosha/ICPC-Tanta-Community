
namespace ICPC_Tanta_Web.Services.BackServices
{
    public class CodeforcesUpdaterHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public CodeforcesUpdaterHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _timer = new Timer(
                async _ => await DoWorkAsync(),
                null,
                TimeSpan.Zero,                   
                TimeSpan.FromDays(7));           

            return Task.CompletedTask;
        }

        private async Task DoWorkAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var updater = scope.ServiceProvider.GetRequiredService<UpdateUserCodeForcesData>();
            await updater.UpdateAllUsersAsync();
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return base.StopAsync(stoppingToken);
        }

        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }
    }
}
