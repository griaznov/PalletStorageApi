using EntityContext.Models;

namespace PalletStorage.WebApi.Repositories;

public interface IBoxRepository
{
    //Task<Box?> CreateAsync(Box c);
    //Task<IEnumerable<Box>> RetrieveAllAsync();
    //Task<Box?> RetrieveAsync(string id);
    //Task<Box?> UpdateAsync(string id, Box c);
    //Task<bool?> DeleteAsync(string id);

    Task<BoxEfModel?> CreateAsync(BoxEfModel box);
    Task<IEnumerable<BoxEfModel>> RetrieveAllAsync();
    Task<BoxEfModel?> RetrieveAsync(string id);
    Task<BoxEfModel?> UpdateAsync(string id, BoxEfModel box);
    Task<bool?> DeleteAsync(string id);
}
