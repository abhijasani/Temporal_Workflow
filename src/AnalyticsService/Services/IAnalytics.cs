namespace AnalyticsService.Services;

public interface IAnalytics
{
    public Task StartAnalytics();

    public Task StopAnalytics();
}
