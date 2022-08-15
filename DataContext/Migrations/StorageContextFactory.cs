using DataContext.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataContext.Migrations;

public class StorageContextFactory : IDesignTimeDbContextFactory<StorageContext>
{
    // TODO - ?
    //public StorageContext CreateDbContext(string[] args)
    StorageContext IDesignTimeDbContextFactory<StorageContext>.CreateDbContext(string[] args)
    {
        var optionsBuilder = CreateDbContextOptions(GetDefaultDataFileName());

        return new StorageContext(optionsBuilder.Options);
    }

    public async Task<IStorageContext> CreateStorageContextAsync(string dataPath)
    {
        // TODO - ?
        //var optionsBuilder = CreateDbContextOptions(dataPath);
        //var dbContext = new StorageContext(optionsBuilder, dataPath);

        var dbContext = new StorageContext(dataPath);

        var dbIsExists = await dbContext.CreateDatabaseAsync(dataPath);

        if (!dbIsExists)
        {
            throw new DbUpdateException($"Error with creating database in {dataPath}");
        }

        return dbContext;
    }

    private DbContextOptionsBuilder<StorageContext> CreateDbContextOptions(string dataPath)
        => new DbContextOptionsBuilder<StorageContext>()
            .UseSqlite($"Data Source={dataPath}")
            .UseLoggerFactory(new ConsoleLoggerFactory());

    private static string GetDefaultDataFileName() => "../PalletStorage.db";

}
