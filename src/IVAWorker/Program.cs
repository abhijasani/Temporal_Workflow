using Temporalio.Client;
using Temporalio.Worker;
using IVAWorker;
using IVAWorker.Workflows;

var client = await TemporalClient.ConnectAsync(new("localhost:7233"));

// Cancellation token to shutdown worker on ctrl+c
using var tokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (_, eventArgs) =>
{
    tokenSource.Cancel();
    eventArgs.Cancel = true;
};

using var worker = new TemporalWorker(
    client, // client
    new TemporalWorkerOptions(taskQueue: "IVA_TASK_QUEUE")
        .AddAllActivities(new LongRunningActivity())
        .AddAllActivities(new NumberPlateActivities())
        .AddWorkflow<MainWorkflow>()
        .AddWorkflow<NumberPlateWorkflow>()
);

Console.WriteLine("Running worker...");
try
{
    await worker.ExecuteAsync(tokenSource.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Worker cancelled");
}