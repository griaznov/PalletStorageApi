using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataContext.Sqlite;

public static class DataContextCreator
{
    /// <summary>
    /// Adds DataContext to the specified IServiceCollection. Uses the Sqlite database provider.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="relativePath">Set to override the default of ".."</param>
    /// <returns>An IServiceCollection that can be used to add more services.</returns>
    public static async Task<IServiceCollection> AddDataContextSqliteAsync(this IServiceCollection services, string relativePath = "..")
    {
        var databasePath = Path.Combine(relativePath, "PalletStorage.db");

        await CreateDataContextAsync(databasePath);

        services.AddDbContext<StorageDataContext>(options =>
            options.UseSqlite($"Data Source={databasePath}")
                .UseLoggerFactory(new ConsoleLoggerFactory())
        );

        return services;
    }

    public static async Task<StorageDataContext> CreateDataContextAsync(string dataPath)
    {
        var db = new StorageDataContext(dataPath);

        if (!File.Exists(dataPath))
        {
            var dbIsCreated = await db.Database.EnsureCreatedAsync();

            if (!dbIsCreated)
            {
                throw new DbUpdateException($"Error with creating data in {dataPath}");
            }
        }

        return db;
    }
}
