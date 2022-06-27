using DataContext;
using DataContext.Models.Converters;
using DataContext.Models.Models;
using Microsoft.EntityFrameworkCore;
using PalletStorage.Common.CommonClasses;

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

    public async Task<List<Pallet>> RetrieveAllAsync(int count = 0, int skip = 0)
    {
        if (count == 0)
        {
            count = 10000;
        }

        return await db.Pallets
            .Skip(skip)
            .Take(count)
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

    public async Task<Pallet?> UpdateAsync(Pallet pallet)
    {
        var palletFounded = await db.Pallets.FindAsync(pallet.Id);

        if (palletFounded is null)
        {
            await db.Pallets.AddAsync(pallet.ToEfModel());
        }
        else
        {
            // update in database
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

    public async Task<bool?> AddBoxToPalletAsync(Box box, int palletId)
    {
        var palletFounded = await db.Pallets.FindAsync(palletId);

        if (palletFounded is null)
        {
            return null;
        }

        var boxEf = await db.Boxes.FindAsync(box.Id);

        if (boxEf is null)
        {
            boxEf = box.ToEfModel();
            await db.Boxes.AddAsync(boxEf);
        }

        boxEf.PalletId = palletId;

        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool?> DeleteBoxFromPalletAsync(Box box)
    {
        var boxEf = await db.Boxes.FindAsync(box.Id);

        if (boxEf == null)
        {
            return null;
        }

        boxEf.PalletId = null;

        return await db.SaveChangesAsync() > 0;
    }
}
