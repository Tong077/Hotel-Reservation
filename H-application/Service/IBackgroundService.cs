namespace H_application.Service
{
    public interface IBackgroundService
    {

        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}
