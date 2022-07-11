using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataContext;
using DataContext.Models.Models;
using Microsoft.EntityFrameworkCore;
using PalletStorage.Common.CommonClasses;

namespace PalletStorage.Repositories.Repositories;

public class PalletRepository : IPalletRepository
{
    // use an instance data context field because it should not be
    // cached due to their internal caching
    private readonly StorageDataContext db;
    private readonly IMapper mapper;

    public PalletRepository(StorageDataContext dbContext, IMapper mapper)
    {
        db = dbContext;
        this.mapper = mapper;
    }

    public async Task<List<Pallet>> GetAllAsync(int take, int skip = 0)
    {
        return await db.Pallets
            .Skip(skip)
            .Take(take)
            .Include(p => p.Boxes)
            .Select(p => mapper.Map<Pallet>(p))
            //.ProjectTo<Pallet>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<Pallet?> GetAsync(int id)
    {
        var palletEf = await db.Pallets.FindAsync(id);

        return mapper.Map<Pallet>(palletEf);
    }

    public async Task<Pallet?> CreateAsync(Pallet pallet)
    {
        // add to database using EF Core
        await db.Pallets.AddAsync(mapper.Map<PalletEfModel>(pallet));

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? pallet : null;
    }

    public async Task<Pallet?> UpdateAsync(Pallet pallet)
    {
        var palletFounded = await db.Pallets.FindAsync(pallet.Id);

        if (palletFounded is null)
        {
            await db.Pallets.AddAsync(mapper.Map<PalletEfModel>(pallet));
        }
        else
        {
            // update in database new values for entry
            mapper.Map(pallet, palletFounded);
        }

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? pallet : null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        // remove from database
        PalletEfModel? palletFounded = await db.Pallets.FindAsync(id);

        if (palletFounded is null)
        {
            return false;
        }

        db.Pallets.Remove(palletFounded);
        var affected = await db.SaveChangesAsync();

        return affected == 1;
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
            boxEf = mapper.Map<BoxEfModel>(box);
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
