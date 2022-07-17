using PalletStorage.Business.Models;

namespace PalletStorage.Repositories.Pallets;

public interface IPalletRepository
{
    Task<PalletModel?> CreateAsync(PalletModel pallet);
    Task<List<PalletModel>> GetAllAsync(int take, int skip = 0);
    Task<PalletModel?> GetAsync(int id);
    Task<PalletModel?> UpdateAsync(PalletModel pallet);
    Task<bool> DeleteAsync(int id);
    Task<int> CountAsync();
    Task<bool?> AddBoxToPalletAsync(BoxModel box, int id);
    Task<bool?> DeleteBoxFromPalletAsync(BoxModel box);
}
