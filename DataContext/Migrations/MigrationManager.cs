using Microsoft.EntityFrameworkCore;

namespace DataContext.Migrations;

public static class MigrationManager
{
    public static async Task MigrateAsync()
    {
        Console.WriteLine("Database migration begin...");

        var contextFactory = new StorageContextFactory();

        await using var dbContext = contextFactory.CreateStorageContext();

        await dbContext.Database.MigrateAsync().ConfigureAwait(false);

        Console.WriteLine("Database migration complete.");
    }
}
