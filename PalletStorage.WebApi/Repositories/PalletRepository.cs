using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.ChangeTracking; // EntityEntry<T>
using DataContext.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using EntityContext.Models;
using PalletStorage.WebApi.Models;

namespace PalletStorage.WebApi.Repositories;

public class PalletRepository : IPalletRepository
{
    // use an instance data context field because it should not be
    // cached due to their internal caching
    private readonly StorageDataContext db;

    public PalletRepository(StorageDataContext injectedContext)
    {
        db = injectedContext;
    }

    public async Task<IEnumerable<PalletEfModel>> RetrieveAllAsync()
    {
        var listBoxes = await db.Pallets.ToListAsync();

        return listBoxes.AsEnumerable();
    }

    public async Task<PalletEfModel?> RetrieveAsync(string idString)
    {
        var id = new Guid(idString);

        return await db.Pallets.FindAsync(id);
    }

    public async Task<PalletEfModel?> CreateAsync(PalletEfModel b)
    {
        // add to database using EF Core
        EntityEntry<PalletEfModel> added = await db.Pallets.AddAsync(b);

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? b : null;
    }

    public async Task<PalletEfModel?> UpdateAsync(string idString, PalletEfModel b)
    {
        var id = new Guid(idString);

        // update in database
        db.Pallets.Update(b);

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? b : null;
    }

    public async Task<bool?> DeleteAsync(string idString)
    {
        var id = new Guid(idString);

        // remove from database
        PalletEfModel? b = await db.Pallets.FindAsync(id);

        if (b is null)
        {
            return null;
        }

        db.Pallets.Remove(b);
        var affected = await db.SaveChangesAsync();

        return affected == 1 ? true : null;
    }

}
