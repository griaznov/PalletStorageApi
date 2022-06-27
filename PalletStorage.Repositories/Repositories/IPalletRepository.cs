using PalletStorage.Common.CommonClasses;

namespace PalletStorage.Repositories.Repositories;

public interface IPalletRepository
{
    Task<Pallet?> CreateAsync(Pallet pallet);
    Task<List<Pallet>> RetrieveAllAsync(int count = 0, int skip = 0);
    Task<Pallet?> RetrieveAsync(int id);
    Task<Pallet?> UpdateAsync(Pallet pallet);
    Task<bool?> DeleteAsync(int id);
    Task<bool?> AddBoxToPalletAsync(Box box, int id);
    Task<bool?> DeleteBoxFromPalletAsync(Box box);
}
