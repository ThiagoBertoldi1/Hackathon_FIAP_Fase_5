using VideoChecker.Data.Repository;
using VideoChecker.Domain.Interfaces.RepositoriesInterfaces;

namespace VideoChecker.API.DI;

public static class RepositoriesDI
{
    public static void AddRepositoriesDI(this IServiceCollection services)
    {
        services.AddTransient<IVideoCheckerRepository, VideoCheckerRepository>();
    }
}
