using DataContext;
using DataContext.Models.Converters;
using DataContext.Models.Models;
using Microsoft.EntityFrameworkCore;
using PalletStorage.Common.CommonClasses;

// EntityEntry<T>

namespace PalletStorage.Repositories.Repositories;

public class PalletRepository : IPalletRepository
{
    // use an instance data context field because it should not be
    // cached due to their internal caching
    private readonly StorageDataContext db;

    public PalletRepository(StorageDataContext injectedContext)
    {
        db = injectedContext;
    }

    public async Task<List<Pallet>> RetrieveAllAsync()
    {
        return await db.Pallets
            .Include(p => p.Boxes)
            .Select(p => p.ToCommonModel())
            .ToListAsync();
    }

    public async Task<Pallet?> RetrieveAsync(int id)
    {
        var palletEf = await db.Pallets.FindAsync(id);

        return palletEf?.ToCommonModel();
    }

    public async Task<Pallet?> CreateAsync(Pallet pallet)
    {
        // add to database using EF Core
        await db.Pallets.AddAsync(pallet.ToEfModel());

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? pallet : null;
    }

    public async Task<Pallet?> UpdateAsync(int id, Pallet pallet)
    {
        BoxEfModel? palletFounded = await db.Boxes.FindAsync(id);

        if (palletFounded is null)
        {
            await db.Pallets.AddAsync(pallet.ToEfModel());
        }
        else
        {
            // update in database
            //db.Pallets.Update(pallet.ToEfModel());
            db.Entry(palletFounded).CurrentValues.SetValues(pallet.ToEfModel());
        }

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? pallet : null;
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        // remove from database
        PalletEfModel? palletFounded = await db.Pallets.FindAsync(id);

        if (palletFounded is null)
        {
            return null;
        }

        db.Pallets.Remove(palletFounded);
        var affected = await db.SaveChangesAsync();

        return affected == 1 ? true : null;
    }

}
