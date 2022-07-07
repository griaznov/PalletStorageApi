using PalletStorage.Common.CommonClasses;

namespace PalletStorage.Repositories.Repositories;

public interface IBoxRepository
{
    Task<Box?> CreateAsync(Box box);
    Task<List<Box>> GetAllAsync(int take = 1000, int skip = 0);
    Task<Box?> GetAsync(int id);
    Task<Box?> UpdateAsync(Box box);
    Task<bool> DeleteAsync(int id);
}
