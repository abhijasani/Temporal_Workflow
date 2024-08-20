using Temporalio.Client;

namespace AnalyticsService.Services;

public class TemporalClientService
{
    public TemporalClient temporalClient {get; set;}

    public TemporalClientService()
    {
      //  temporalClient = TemporalClient.ConnectAsync(new("localhost:7233"));
        temporalClient = TemporalClient.ConnectAsync(new("localhost:7233")).GetAwaiter().GetResult();
    }
}
