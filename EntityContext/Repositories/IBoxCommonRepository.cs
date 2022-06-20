using EntityContext.Models.Models;

namespace EntityContext.Repositories;

public interface IBoxCommonRepository
{
    Task<BoxEfModel?> CreateAsync(BoxEfModel box);
    Task<IEnumerable<BoxEfModel>> RetrieveAllAsync();
    Task<BoxEfModel?> RetrieveAsync(string id);
    Task<BoxEfModel?> UpdateAsync(string id, BoxEfModel box);
    Task<bool?> DeleteAsync(string id);
}
