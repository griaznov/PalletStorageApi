using PalletStorage.Common.CommonClasses;

namespace PalletStorage.Repositories.Repositories;

public interface IBoxRepository
{
    Task<Box?> CreateAsync(Box box);
    Task<List<Box>> RetrieveAllAsync(int count = 0, int skip = 0);
    Task<Box?> RetrieveAsync(int id);
    Task<Box?> UpdateAsync(Box box);
    Task<bool?> DeleteAsync(int id);
}
