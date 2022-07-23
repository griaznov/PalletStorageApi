using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataContext.Extensions;

public static class ServiceCollectionExtensions
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

        await using var dbContext = await CreateStorageContextAsync(databasePath);

        services.AddDbContext<IStorageContext, StorageContext>(options =>
            options.UseSqlite($"Data Source={databasePath}")
                .UseLoggerFactory(new ConsoleLoggerFactory())
        );

        return services;
    }

    private static async Task<IStorageContext> CreateStorageContextAsync(string dataPath)
    {
        var dbContext = new StorageContext(dataPath);

        var dbIsExists = await dbContext.CreateDatabaseAsync(dataPath);

        if (!dbIsExists)
        {
            throw new DbUpdateException($"Error with creating database in {dataPath}");
        }

        return dbContext;
    }
}
