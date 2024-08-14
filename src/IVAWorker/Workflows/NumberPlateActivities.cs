using IVAWorker.Services;
using Temporalio.Activities;

namespace IVAWorker.Workflows;

public class NumberPlateActivities
{
    private readonly WebAPIService _webAPIService;

    public NumberPlateActivities()
    {
        _webAPIService = new WebAPIService();
    }

    [Activity]
    public async Task<string> ValidateNumberPlate(string numberPlate)
    {
        return await _webAPIService.ValidateNumberPlateAPICall(numberPlate);
    }
}
