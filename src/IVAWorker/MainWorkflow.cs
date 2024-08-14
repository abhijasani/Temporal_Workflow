using IVAWorker.Workflows;
using Temporalio.Common;
using Temporalio.Workflows;

namespace IVAWorker;

[Workflow]
public class MainWorkflow
{
    [WorkflowRun]
    public async Task<string> StartMain(Guid employeeId)
    {
        var retryPolicy = new RetryPolicy
        {
            InitialInterval = TimeSpan.FromSeconds(10),
            MaximumInterval = TimeSpan.FromSeconds(20),
            BackoffCoefficient = 2,
            MaximumAttempts = 2,
        };

        await Workflow.ExecuteActivityAsync(
            (LongRunningActivity activity)
            => activity.StartLongRunningActivity(),
            new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5), RetryPolicy = retryPolicy }
        );

        return "Done!";
        
    }

    [WorkflowSignal]
    public async Task NumberPlateSignal(string numberPlate)
    {
        await Workflow.ExecuteChildWorkflowAsync(
        (NumberPlateWorkflow wf) => wf.RunAsync(numberPlate),
            new ChildWorkflowOptions { Id = $"{nameof(NumberPlateWorkflow)}-{numberPlate}" }
        );
    }
}
