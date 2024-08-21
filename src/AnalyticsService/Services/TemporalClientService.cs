using Temporalio.Client;

namespace AnalyticsService.Services;

public class TemporalClientService
{
    public TemporalClient temporalClient { get; set; }

    public TemporalClientService(IConfiguration configuration)
    {
        var serviceAddress = configuration["Temporal:ServiceAddress"] ?? "localhost:7233";

        temporalClient = TemporalClient.ConnectAsync(new(serviceAddress)).GetAwaiter().GetResult();
    }
}
