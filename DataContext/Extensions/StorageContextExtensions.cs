namespace DataContext.Extensions;

public static class StorageContextExtensions
{
    public static async Task<bool> CreateDatabaseAsync(this IStorageContext dbContext, string dataPath)
    {
        if (!File.Exists(dataPath))
        {
            return await dbContext.Database.EnsureCreatedAsync();
        }

        return true;
    }
}
