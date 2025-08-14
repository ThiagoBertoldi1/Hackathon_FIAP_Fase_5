namespace VideoChecker.Processor;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Waiting messages...");

        while (!stoppingToken.IsCancellationRequested) await Task.Delay(1000, stoppingToken);
    }
}