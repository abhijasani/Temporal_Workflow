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

            await foreach (string numberPlate in _numberPlateRecognitionService.StartAnalytics
                (cameraId, cancellationToken))
            {
                Console.WriteLine($"Detected Number Plate {numberPlate}");

                var workflowDescription = await handle.DescribeAsync();

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
            }

            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

    }

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
