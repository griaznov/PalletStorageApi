using EntityContext.Models.Models;

namespace PalletStorage.WebApi.Repositories;

public interface IPalletRepository
{
    Task<PalletEfModel?> CreateAsync(PalletEfModel pallet);
    Task<IEnumerable<PalletEfModel>> RetrieveAllAsync();
    Task<PalletEfModel?> RetrieveAsync(string id);
    Task<PalletEfModel?> UpdateAsync(string id, PalletEfModel pallet);
    Task<bool?> DeleteAsync(string id);
}
