using Temporalio.Activities;

namespace IVAWorker;

public class LongRunningActivity
{
    [Activity]
    public async Task<string> StartLongRunningActivity()
    {
        await Task.Delay(1000000);
        return "Completed";
    }
}
