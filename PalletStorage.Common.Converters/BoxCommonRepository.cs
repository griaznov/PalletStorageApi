
using PalletStorage.Common.CommonClasses;

namespace PalletStorage.Common.Controllers;

public class BoxCommonRepository : IBoxCommonRepository
{
    //public BoxRepoBoxCommonRepositorysitory(StorageDataContext injectedContext)
    //{
    //    db = injectedContext;

    //    if (boxesCache is null)
    //    {
    //        boxesCache = new ConcurrentDictionary<Guid, BoxEfModel>(
    //            db.Boxes.ToDictionary(c => c.Id));
    //    }
    //}

    public BoxCommonRepository()
    {
    }

    public async Task<IEnumerable<Box>> RetrieveAllAsync()
    {
        var listBoxes = new List<Box>();

        return listBoxes.AsEnumerable();
    }

}
