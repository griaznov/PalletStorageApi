using PalletStorage.Common.CommonClasses;

namespace PalletStorage.Repositories.Repositories;

public interface IBoxRepository
{
    Task<Box?> CreateAsync(Box box);
    Task<List<Box>> RetrieveAllAsync();
    Task<Box?> RetrieveAsync(int id);
    Task<Box?> UpdateAsync(int id, Box box);
    Task<bool?> DeleteAsync(int id);
}
