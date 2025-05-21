using Microsoft.Extensions.DependencyInjection;
using WebNovel.Services.Interfaces;

namespace WebNovel.Services
{
    public class QueuedHostedService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<QueuedHostedService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IServiceProvider _serviceProvider;
        public QueuedHostedService(
            IBackgroundTaskQueue taskQueue,
            ILogger<QueuedHostedService> logger,
            IServiceProvider serviceProvider,
            IServiceScopeFactory serviceScopeFactory)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _serviceProvider = serviceProvider;
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    _logger.LogInformation("Queued Hosted Service is starting.");

        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        var workItem = await _taskQueue.DequeueAsync(stoppingToken);

        //        using (var scope = _serviceScopeFactory.CreateScope())
        //        {
        //            try
        //            {
        //                await workItem(stoppingToken);
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error occurred executing background task.");
        //            }
        //        }
        //    }
        //}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await _taskQueue.DequeueV2Async(stoppingToken);

                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    await workItem(scope.ServiceProvider, stoppingToken);
                }
                catch (Exception ex)
                {
                    // TODO: Ghi log nếu cần
                    Console.WriteLine($"[Background Error] {ex.Message}");
                }
            }
        }
    }

}
