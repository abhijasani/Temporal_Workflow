using IVAWorker.Services;
using Microsoft.Extensions.Configuration;
using Temporalio.Activities;

namespace IVAWorker.Workflows;

public class NumberPlateActivities
{
    private readonly WebAPIService _webAPIService;

    public NumberPlateActivities(IConfiguration configuration)
    {
        _webAPIService = new WebAPIService(configuration);
    }

    [Activity]
    public async Task<string> ValidateNumberPlate(string numberPlate)
    {
        return await _webAPIService.ValidateNumberPlateAPICall(numberPlate);
    }
}
