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
    /// <param name="databasePath">Set to override the default of ".."</param>
    /// <returns>An IServiceCollection that can be used to add more services.</returns>
    public static async Task<IServiceCollection> AddStorageDataContextAsync(this IServiceCollection services, string databasePath = "")
    {
        // TODO ?
        if (string.IsNullOrEmpty(databasePath))
        {
            databasePath = "../PalletStorage.db";
        }

        var contextFactory = new StorageContextFactory();

        await using var dbContext = await contextFactory.CreateStorageContextAsync(databasePath);

        services.AddDbContext<IStorageContext, StorageContext>(options =>
            options.UseSqlite($"Data Source={databasePath}")
                .UseLoggerFactory(new ConsoleLoggerFactory())
        );

        return services;
    }
}
