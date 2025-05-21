namespace WebNovel.Services.Interfaces
{
    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundTask(Func<CancellationToken, Task> task);
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
        void QueueBackgroundWorkItem(Func<IServiceProvider, CancellationToken, Task> workItem);
        Task<Func<IServiceProvider, CancellationToken, Task>> DequeueV2Async(CancellationToken cancellationToken);
    }
}
