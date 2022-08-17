using DataContext.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataContext.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds DataContext to the specified IServiceCollection. Uses the Sqlite database provider.
    /// </summary>
    /// <param name="services"></param>
    /// <returns>An IServiceCollection that can be used to add more services.</returns>
    public static IServiceCollection AddStorageDataContext(this IServiceCollection services)
    {
        services.AddDbContext<IStorageContext, StorageContext>();

        return services;
    }
}
