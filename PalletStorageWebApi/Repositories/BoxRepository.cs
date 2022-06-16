using System.Collections.Concurrent;
using EntityModelsSqlite;
using DataContextSqlite;

namespace PalletStorageWebApi.Repositories;

public class BoxRepository : IBoxRepository
{
    // use a static thread-safe dictionary field to cache
    private static ConcurrentDictionary<Guid, Box>? customersCache;

    // use an instance data context field because it should not be
    // cached due to their internal caching
    private readonly DataContext db;

    public BoxRepository(DataContext injectedContext)
    {
        db = injectedContext;

        if (customersCache is null)
        {
            customersCache = new ConcurrentDictionary<Guid, Box>(
                db.Boxes.ToDictionary(c => c.Id));
        }
    }

    public Task<IEnumerable<Box>> RetrieveAllAsync()
    {
        // for performance, get from cache
        return Task.FromResult(customersCache is null ? Enumerable.Empty<Box>() : customersCache.Values);
    }

}
