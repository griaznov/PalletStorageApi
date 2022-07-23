using PalletStorage.BusinessModels;

namespace PalletStorage.Repositories.Boxes;

public interface IBoxRepository
{
    Task<BoxModel?> CreateAsync(BoxModel box, CancellationToken token = default);
    Task<IReadOnlyCollection<BoxModel>> GetAllAsync(int take, int skip, CancellationToken token = default);
    Task<BoxModel?> GetAsync(int id, CancellationToken token = default);
    Task<BoxModel?> UpdateAsync(BoxModel box, CancellationToken token = default);
    Task<bool> DeleteAsync(int id, CancellationToken token = default);
    Task<int> CountAsync();
}
