using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.ChangeTracking; // EntityEntry<T>
using DataContext.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using EntityContext.Models;
using PalletStorage.WebApi.Models;

namespace PalletStorage.WebApi.Repositories;

public class BoxRepository : IBoxRepository
{
    // use a static thread-safe dictionary field to cache
    private static ConcurrentDictionary<Guid, BoxEfModel>? boxesCache;
    
    // use an instance data context field because it should not be
    // cached due to their internal caching
    private readonly StorageDataContext db;

    public BoxRepository(StorageDataContext injectedContext)
    {
        db = injectedContext;

        if (boxesCache is null)
        {
            boxesCache = new ConcurrentDictionary<Guid, BoxEfModel>(
                db.Boxes.ToDictionary(c => c.Id));
        }
    }

    private BoxEfModel UpdateCache(Guid id, BoxEfModel b)
    {
        BoxEfModel? old;

        if (boxesCache is not null)
        {
            if (boxesCache.TryGetValue(id, out old))
            {
                if (boxesCache.TryUpdate(id, b, old))
                {
                    return b;
                }
            }
        }
        return null!;
    }

    public async Task<IEnumerable<BoxEfModel>> RetrieveAllAsync()
    {
        // for performance, get from cache
        //return Task.FromResult(boxesCache is null ? Enumerable.Empty<BoxEfModel>() : boxesCache.Values);

        var listBoxes = await db.Boxes.ToListAsync();

        return listBoxes.AsEnumerable();
    }

    public async Task<BoxEfModel?> RetrieveAsync(string idString)
    {
        var id = new Guid(idString);

        return await db.Boxes.FindAsync(id);

        //if (boxesCache is null)
        //{
        //    return null!;
        //}
        //// for performance, get from cache
        //boxesCache.TryGetValue(id, out BoxEfModel? b);
        //return Task.FromResult(b);
    }

    public async Task<BoxEfModel?> CreateAsync(BoxEfModel b)
    {
        // add to database using EF Core
        EntityEntry<BoxEfModel> added = await db.Boxes.AddAsync(b);

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? b : null;

        //if (affected == 1)
        //{
        //    // if the Box is new, add it to cache, else call UpdateCache method
        //    return boxesCache is null ? b : boxesCache.AddOrUpdate(b.Id, b, UpdateCache);
        //}
        //else
        //{
        //    return null;
        //}
    }

    public async Task<BoxEfModel?> UpdateAsync(string idString, BoxEfModel b)
    {
        var id = new Guid(idString);

        // update in database
        db.Boxes.Update(b);

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? b : null;

        //return affected == 1 ? UpdateCache(id, b) : null;
    }

    public async Task<bool?> DeleteAsync(string idString)
    {
        var id = new Guid(idString);

        // remove from database
        BoxEfModel? b = await db.Boxes.FindAsync(id);

        if (b is null)
        {
            return null;
        }

        db.Boxes.Remove(b);
        var affected = await db.SaveChangesAsync();

        return affected == 1 ? true : null;

        //return affected == 1 ? boxesCache?.TryRemove(id, out b) : null;
    }

}
