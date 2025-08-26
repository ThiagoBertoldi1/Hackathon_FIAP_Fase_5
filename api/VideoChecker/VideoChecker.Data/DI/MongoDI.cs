using Microsoft.Extensions.DependencyInjection;

namespace VideoChecker.Data.DI;

public static class MongoDI
{
    public static void AddMongoDI(this IServiceCollection service)
        => service.AddSingleton<IMongoService, MongoService>();
}
