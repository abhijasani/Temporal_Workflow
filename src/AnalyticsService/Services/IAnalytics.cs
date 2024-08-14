namespace AnalyticsService.Services;

public interface IAnalytics
{
    public IAsyncEnumerable<string> StartAnalytics(Guid CameraId, CancellationToken cancellationToken);

    public Task StopAnalytics();
}
