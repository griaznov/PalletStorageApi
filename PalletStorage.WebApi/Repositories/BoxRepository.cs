using System.Collections.Concurrent;
using EntityModels.Sqlite;

namespace PalletStorage.WebApi.Repositories;

public class BoxRepository : IBoxRepository
{
    // use a static thread-safe dictionary field to cache
    private static ConcurrentDictionary<Guid, BoxModel>? customersCache;

    // use an instance data context field because it should not be
    // cached due to their internal caching
    private readonly DataContext.Sqlite.DataContext db;

    public BoxRepository(DataContext.Sqlite.DataContext injectedContext)
    {
        db = injectedContext;

        if (customersCache is null)
        {
            customersCache = new ConcurrentDictionary<Guid, BoxModel>(
                db.Boxes.ToDictionary(c => c.Id));
        }
    }

    public Task<IEnumerable<BoxModel>> RetrieveAllAsync()
    {
        // for performance, get from cache
        return Task.FromResult(customersCache is null ? Enumerable.Empty<BoxModel>() : customersCache.Values);
    }

}
