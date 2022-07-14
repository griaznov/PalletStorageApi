using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataContext;

public static class StorageContextCreator
{
    /// <summary>
    /// Adds DataContext to the specified IServiceCollection. Uses the Sqlite database provider.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="relativePath">Set to override the default of ".."</param>
    /// <returns>An IServiceCollection that can be used to add more services.</returns>
    public static async Task<IServiceCollection> AddStorageDataContextAsync(this IServiceCollection services, string relativePath = "..")
    {
        var databasePath = Path.Combine(relativePath, "PalletStorage.db");

        await using var context = await StorageContext.CreateContextAsync(databasePath);

        services.AddDbContext<IStorageContext, StorageContext>(options =>
            options.UseSqlite($"Data Source={databasePath}")
                .UseLoggerFactory(new ConsoleLoggerFactory())
        );

        return services;
    }
}
