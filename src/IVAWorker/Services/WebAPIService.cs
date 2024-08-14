namespace IVAWorker.Services;

public class WebAPIService
{
    private const string VehicleManagementServiceUrl = "http://localhost:5048";
    private readonly HttpClient _httpClient;

    public WebAPIService()
    {
        _httpClient = new HttpClient();
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
