using System.Runtime.CompilerServices;

namespace AnalyticsService.Services;

public class NumberPlateRecognitionService : IAnalytics
{
    private CancellationTokenSource? _cancellationTokenSource;

    public async IAsyncEnumerable<string> StartAnalytics(Guid CameraId, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        var random = new Random();
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            await Task.Delay(random.Next(10000, 30000), cancellationToken);

            var numberPlate = GenerateRandomNumberPlate(random);

            yield return numberPlate;
        }
    }

    public Task StopAnalytics()
    {
        _cancellationTokenSource?.Cancel();
        return Task.CompletedTask;
    }

    private static string GenerateRandomNumberPlate(Random random)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 7)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
