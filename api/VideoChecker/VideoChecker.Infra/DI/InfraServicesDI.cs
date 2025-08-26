using Microsoft.Extensions.DependencyInjection;
using VideoChecker.Infra.RabbitMQ;

namespace VideoChecker.Infra.DI;

public static class InfraServicesDI
{
    public static void AddInfraDI(this IServiceCollection services)
    {
        services.AddSingleton<IQueueService, QueueService>();
    }
}
