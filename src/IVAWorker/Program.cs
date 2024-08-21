using Temporalio.Client;
using Temporalio.Worker;
using IVAWorker;
using IVAWorker.Workflows;
using Microsoft.Extensions.Configuration;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

var builder = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Base settings
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true); // Environment-specific settings

var configuration = builder.Build();

var serviceAddress = configuration["Temporal:ServiceAddress"] ?? "localhost:7233";

var client = await TemporalClient.ConnectAsync(new(serviceAddress));

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
        .AddAllActivities(new NumberPlateActivities(configuration))
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