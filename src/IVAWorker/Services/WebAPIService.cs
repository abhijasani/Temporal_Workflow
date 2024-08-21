using Microsoft.Extensions.Configuration;

namespace IVAWorker.Services;

public class WebAPIService
{
    private readonly string VehicleManagementServiceUrl;
    private readonly HttpClient _httpClient;

    public WebAPIService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        VehicleManagementServiceUrl = 
            configuration["VehicleManagementService:BaseUrl"]  ?? "http://localhost:5048"; 
    }

    public async Task<string> ValidateNumberPlateAPICall(string numberPlate)
    {
        string endpoint = $"{VehicleManagementServiceUrl}/api/VehicleManagement?numberPlate={numberPlate}";
        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

        if (response.IsSuccessStatusCode)
        {
            string responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
        else
        {
            Console.WriteLine($"Request failed with status code: {response.StatusCode}");
            return string.Empty;
        }
    }

}
