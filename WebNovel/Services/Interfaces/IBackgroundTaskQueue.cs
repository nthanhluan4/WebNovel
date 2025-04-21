namespace WebNovel.Services.Interfaces
{
    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundTask(Func<CancellationToken, Task> task);
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}
