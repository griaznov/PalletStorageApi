using PalletStorage.Common.CommonClasses;

namespace PalletStorage.Repositories.Repositories;

public interface IPalletRepository
{
    Task<Pallet?> CreateAsync(Pallet pallet);
    Task<List<Pallet>> RetrieveAllAsync();
    Task<Pallet?> RetrieveAsync(int id);
    //Task<Pallet?> UpdateAsync(int id, Pallet pallet);
    Task<Pallet?> UpdateAsync(Pallet pallet);
    Task<bool?> DeleteAsync(int id);
    Task<bool?> AddBoxToPalletAsync(Box box, Pallet pallet);
    Task<bool?> DeleteBoxFromPallet(Box box);
}
