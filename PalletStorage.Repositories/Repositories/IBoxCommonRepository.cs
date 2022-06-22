using PalletStorage.Common.CommonClasses;

namespace PalletStorage.Repositories.Repositories;

public interface IBoxCommonRepository
{
    Task<Box?> CreateAsync(Box box);
    //Task<IEnumerable<BoxEfModel>> RetrieveAllAsync();
    Task<List<Box>> RetrieveAllAsync();
    Task<Box?> RetrieveAsync(string id);
    Task<Box?> UpdateAsync(string id, Box box);
    Task<bool?> DeleteAsync(string id);
}
