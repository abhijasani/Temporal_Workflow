using AnalyticsService.Services;
using Microsoft.AspNetCore.Mvc;
using Temporalio.Client;
using IVAWorker;

namespace AnalyticsService.Controllers;


[Route("api/[controller]")]
[ApiController]
public class NumberPlateRecognitionController : ControllerBase
{
    private readonly NumberPlateRecognitionService _numberPlateRecognitionService;
    private TemporalClient? _temporalClient;

    public NumberPlateRecognitionController(NumberPlateRecognitionService numberPlateRecognitionService)
    {
        _numberPlateRecognitionService = numberPlateRecognitionService;

    }

    [HttpGet("start")]
    public async Task<IActionResult> StartNumberPlateRecognition(Guid cameraId, CancellationToken cancellationToken)
    {
        _temporalClient = await TemporalClient.ConnectAsync(new("localhost:7233"));

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

                await handle.SignalAsync(wf => wf.NumberPlateSignal(numberPlate));
            }

            return Ok();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

    }

    [HttpPost("stop")]
    public async Task StopNUmberPlatRecognition()
    {
        await _numberPlateRecognitionService.StopAnalytics();
    }
}
