using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataContext.Extensions;

internal static class StorageContextExtensions
{
    internal static async Task<bool> CreateDatabaseAsync(this IStorageContext dbContext, string dataPath)
    {
        // TODO ?
        //if (!await dbContext.Database.GetService<IRelationalDatabaseCreator>().ExistsAsync())
        if (!File.Exists(dataPath))
        {
            // TODO ?
            // return await dbContext.Database.EnsureCreatedAsync();
            await dbContext.Database.MigrateAsync();
        }

        return true;
    }
}
