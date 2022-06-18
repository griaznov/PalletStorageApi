using System.Collections.Concurrent;
using DataContext.Sqlite;
using EntityContext.Sqlite;

namespace PalletStorage.WebApi.Repositories;

public class BoxRepository : IBoxRepository
{
    // use a static thread-safe dictionary field to cache
    private static ConcurrentDictionary<Guid, BoxEfModel>? customersCache;

    // use an instance data context field because it should not be
    // cached due to their internal caching
    private readonly StorageDataContext db;

    public BoxRepository(StorageDataContext injectedContext)
    {
        db = injectedContext;

        if (customersCache is null)
        {
            customersCache = new ConcurrentDictionary<Guid, BoxEfModel>(
                db.Boxes.ToDictionary(c => c.Id));
        }
    }

    public Task<IEnumerable<BoxEfModel>> RetrieveAllAsync()
    {
        // for performance, get from cache
        return Task.FromResult(customersCache is null ? Enumerable.Empty<BoxEfModel>() : customersCache.Values);
    }

}
