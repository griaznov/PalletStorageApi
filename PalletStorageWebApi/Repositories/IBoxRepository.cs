using EntityModelsSqlite;

namespace PalletStorageWebApi.Repositories;

public interface IBoxRepository
{
    //Task<Box?> CreateAsync(Box c);
    //Task<IEnumerable<Box>> RetrieveAllAsync();
    //Task<Box?> RetrieveAsync(string id);
    //Task<Box?> UpdateAsync(string id, Box c);
    //Task<bool?> DeleteAsync(string id);

    //Task<Box?> CreateAsync(Box c);
    Task<IEnumerable<Box>> RetrieveAllAsync();
    //Task<Box?> RetrieveAsync(Guid id);
    //Task<Box?> UpdateAsync(Guid id, Box c);
    //Task<bool?> DeleteAsync(Guid id);
}
