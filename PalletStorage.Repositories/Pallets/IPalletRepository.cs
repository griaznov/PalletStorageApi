using PalletStorage.BusinessModels;

namespace PalletStorage.Repositories.Pallets;

public interface IPalletRepository
{
    Task<PalletModel?> CreateAsync(PalletModel pallet, CancellationToken token = default);
    Task<List<PalletModel>> GetAllAsync(int take, int skip, CancellationToken token = default);
    Task<PalletModel?> GetAsync(int id, CancellationToken token = default);
    Task<PalletModel?> UpdateAsync(PalletModel pallet, CancellationToken token = default);
    Task<bool> DeleteAsync(int id, CancellationToken token = default);
    Task<int> CountAsync();
    Task<bool?> AddBoxToPalletAsync(BoxModel box, int id, CancellationToken token = default);
    Task<bool?> DeleteBoxFromPalletAsync(BoxModel box, CancellationToken token = default);
}
