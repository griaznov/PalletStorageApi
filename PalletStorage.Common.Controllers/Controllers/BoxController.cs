using EntityContext.Models.Converters;
using EntityContext.Models.Models;
using EntityContext.Repositories;
using PalletStorage.Common.CommonClasses;
using PalletStorage.WebApi.Models.Converters;
using PalletStorage.WebApi.Models.Models;

namespace PalletStorage.Common.Controllers.Controllers;

// base address: api/boxes

public class BoxesController
{
    private readonly IBoxCommonRepository repo;

    // constructor injects repository registered in Startup
    public BoxesController(IBoxCommonRepository repo)
    {
        this.repo = repo;
    }

    public async Task<IEnumerable<BoxEfModel>> GetBoxes()
    {
        return await repo.RetrieveAllAsync();
    }

    public async Task<Box?> GetBox(string id)
    {
        BoxEfModel? box = await repo.RetrieveAsync(id);

        return box?.ToCommonModel();
    }

    public async Task<Box?> Create(Box box)
    {
        BoxEfModel? addedBox = await repo.CreateAsync(box.ToEfModel());

        return addedBox?.ToCommonModel();
    }

    public async Task<Box?> Update(string id, Box box)
    {
        BoxEfModel? existingBox = await repo.RetrieveAsync(id);

        if (existingBox == null)
        {
            return null;
        }

        BoxEfModel? updBox = await repo.UpdateAsync(id, box.ToEfModel());

        return updBox?.ToCommonModel();
    }

    public async Task<bool?> Delete(string id)
    {
        BoxEfModel? existing = await repo.RetrieveAsync(id);

        if (existing == null)
        {
            return null;
        }

        var deleted = await repo.DeleteAsync(id);

        return deleted;
    }
}
