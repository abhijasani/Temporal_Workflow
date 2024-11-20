using AnalyticsService.Services;
using Microsoft.AspNetCore.Mvc;
using Temporalio.Client;
using IVAWorker;
using Temporalio.Api.Enums.V1;

namespace AnalyticsService.Controllers;


[Route("api/[controller]")]
[ApiController]
public class NumberPlateRecognitionController : ControllerBase
{
    private readonly NumberPlateRecognitionService _numberPlateRecognitionService;
    private TemporalClient _temporalClient;

    static int TotalNumberPlate = 0;

    public NumberPlateRecognitionController(NumberPlateRecognitionService numberPlateRecognitionService,
        TemporalClientService temporalClientService)
    {
        _numberPlateRecognitionService = numberPlateRecognitionService;
        _temporalClient = temporalClientService.temporalClient;
    }


    [HttpGet("start")]
    public async Task<IActionResult> StartNumberPlateRecognition(Guid cameraId, CancellationToken cancellationToken)
    {
        var workflowId = $"{nameof(MainWorkflow)}-{Guid.NewGuid()}";

        try
        {
            var handle = await _temporalClient.StartWorkflowAsync(
               (MainWorkflow wf) => wf.StartMain(cameraId),
               new(id: workflowId, taskQueue: "IVA_TASK_QUEUE"));

            TotalNumberPlate = 0;
            await foreach (string numberPlate in _numberPlateRecognitionService.StartAnalytics
                (cameraId, cancellationToken))
            {
                Console.WriteLine($"Detected Number Plate {numberPlate}");

                var workflowDescription = await handle.DescribeAsync();
                TotalNumberPlate++;
                Console.WriteLine(TotalNumberPlate);

                if (workflowDescription.Status == WorkflowExecutionStatus.Running)
                {
                    // Send the signal
                    await handle.SignalAsync(wf => wf.NumberPlateSignal(numberPlate));
                }
                else
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return Ok($"Workflow Terminated Successfully");
                    }

                    return BadRequest($"Workflow is not running. Current status: {workflowDescription.Status}");
                }


                // For testing 
                // if (TotalNumberPlate >= 10)
                // {
                //     await handle.TerminateAsync();
                //     return Ok("10 Numberplate complated");
                // }
            }
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

// For Load Testing
// [HttpGet("start")]
// public async Task<IActionResult> StartNumberPlateRecognition(int numberOfWorkflows, Guid cameraId, CancellationToken cancellationToken)
// {
//     var tasks = new List<Task>();

//     try
//     {
//         for (int i = 0; i < numberOfWorkflows; i++)
//         {
//             var workflowId = $"{nameof(MainWorkflow)}-{Guid.NewGuid()}";
//             var task = Task.Run(async () =>
//             {
//                 var handle = await _temporalClient.StartWorkflowAsync(
//                     (MainWorkflow wf) => wf.StartMain(cameraId),
//                     new(id: workflowId, taskQueue: "IVA_TASK_QUEUE"));

//                 await foreach (string numberPlate in _numberPlateRecognitionService.StartAnalytics(cameraId, cancellationToken))
//                 {
//                     Console.WriteLine($"Detected Number Plate {numberPlate}");

//                     var workflowDescription = await handle.DescribeAsync();

//                     if (workflowDescription.Status == WorkflowExecutionStatus.Running)
//                     {
//                         await handle.SignalAsync(wf => wf.NumberPlateSignal(numberPlate));
//                     }
//                     else
//                     {
//                         if (cancellationToken.IsCancellationRequested)
//                         {
//                             Console.WriteLine($"Workflow {workflowId} Terminated Successfully");
//                             return;
//                         }

//                         Console.WriteLine($"Workflow {workflowId} is not running. Current status: {workflowDescription.Status}");
//                         return;
//                     }
//                 }
//             }, cancellationToken);

//             tasks.Add(task);
//         }

//         await Task.WhenAll(tasks);
//         return Ok("All workflows started successfully.");
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine(ex.Message);
//         return StatusCode(500, "An error occurred while starting workflows.");
//     }
// }


    [HttpGet("stop")]
    public async Task StopNumberPlateRecognition(string workflowId)
    {
        await _numberPlateRecognitionService.StopAnalytics();

        if (workflowId != null)
        {
            WorkflowHandle? handle = _temporalClient?.GetWorkflowHandle(workflowId);

            if (handle != null)
            {
                await handle.TerminateAsync();
            }
        }
    }
}
