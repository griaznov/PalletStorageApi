using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataContext.Migrations;

internal class StorageContextFactory : IDesignTimeDbContextFactory<StorageContext>
{
    private const string DefaultDataFileName = "../PalletStorage.db";

    StorageContext IDesignTimeDbContextFactory<StorageContext>.CreateDbContext(string[] args)
    {
        return CreateStorageContext();
    }

    public StorageContext CreateStorageContext(string dataPath = DefaultDataFileName)
    {
        var optionsBuilder = CreateDbContextOptions(dataPath);

        var dbContext = new StorageContext(optionsBuilder.Options, dataPath);
        
        return dbContext;
    }

    private static DbContextOptionsBuilder<StorageContext> CreateDbContextOptions(string dataPath)
        => new DbContextOptionsBuilder<StorageContext>()
            .UseSqlite($"Data Source={dataPath}")
            .UseLoggerFactory(new ConsoleLoggerFactory());
}
