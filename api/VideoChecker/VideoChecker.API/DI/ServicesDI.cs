using VideoChecker.Domain.Interfaces.ServicesInterfaces;
using VideoChecker.Domain.Services;

namespace VideoChecker.API.DI;

public static class ServicesDI
{
    public static void AddServicesDI(this IServiceCollection services)
    {
        services.AddTransient<IVideoCheckerService, VideoCheckerService>();
    }
}
