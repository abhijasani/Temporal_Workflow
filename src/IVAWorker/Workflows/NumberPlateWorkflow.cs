using Temporalio.Common;
using Temporalio.Workflows;

namespace IVAWorker.Workflows;

[Workflow]
public class NumberPlateWorkflow
{
    [WorkflowRun]
    public async Task<string> RunAsync(string numberPlate)
    {
        var retryPolicy = new RetryPolicy
        {
            InitialInterval = TimeSpan.FromSeconds(10),
            MaximumInterval = TimeSpan.FromSeconds(20),
            BackoffCoefficient = 2,
            MaximumAttempts = 2,
        };

        var result = await Workflow.ExecuteActivityAsync(
            (NumberPlateActivities activity)
            => activity.ValidateNumberPlate(numberPlate),
            new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5), RetryPolicy = retryPolicy }
        );  

        if(result == "true")
        {
            return "Send Notification to User";
        }

        return "Send Notification to Security and Start Recording";
    }
}
