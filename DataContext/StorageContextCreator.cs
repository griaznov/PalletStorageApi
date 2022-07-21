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

        await using var context = await CreateContextAsync(databasePath);

        services.AddDbContext<IStorageContext, StorageContext>(options =>
            options.UseSqlite($"Data Source={databasePath}")
                .UseLoggerFactory(new ConsoleLoggerFactory())
        );

        return services;
    }

    public static async Task<IStorageContext> CreateContextAsync(string dataPath)
    {
        var db = new StorageContext(dataPath);

        var dbIsExists = await db.CreateDatabaseAsync(dataPath);

        if (!dbIsExists)
        {
            throw new DbUpdateException($"Error with creating database in {dataPath}");
        }

        return db;
    }

    public static async Task<bool> CreateDatabaseAsync(this IStorageContext context, string dataPath)
    {
        if (!File.Exists(dataPath))
        {
            return await context.Database.EnsureCreatedAsync();
        }

        return true;
    }
}
