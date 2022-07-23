namespace DataContext.Extensions;

internal static class StorageContextExtensions
{
    internal static async Task<bool> CreateDatabaseAsync(this IStorageContext dbContext, string dataPath)
    {
        if (!File.Exists(dataPath))
        {
            return await dbContext.Database.EnsureCreatedAsync();
        }

        return true;
    }
}
