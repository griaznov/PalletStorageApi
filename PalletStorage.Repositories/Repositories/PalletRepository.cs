﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataContext;
using DataContext.Models.Entities;
using Microsoft.EntityFrameworkCore;
using PalletStorage.Business.Models;

namespace PalletStorage.Repositories.Repositories;

public class PalletRepository : IPalletRepository
{
    // use an instance data context field because it should not be
    // cached due to their internal caching
    private readonly IStorageContext db;
    private readonly IMapper mapper;

    public PalletRepository(IStorageContext storageContext, IMapper mapper)
    {
        db = storageContext;
        this.mapper = mapper;
    }

    public async Task<List<PalletModel>> GetAllAsync(int take, int skip = 0)
    {
        return await db.Pallets
            .Skip(skip)
            .Take(take)
            .Include(p => p.Boxes)
            .ProjectTo<PalletModel>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<PalletModel?> GetAsync(int id)
    {
        var palletEf = await db.Pallets.FindAsync(id);

        return mapper.Map<PalletModel>(palletEf);
    }

    public async Task<PalletModel?> CreateAsync(PalletModel pallet)
    {
        // add to database using EF Core
        await db.Pallets.AddAsync(mapper.Map<Pallet>(pallet));

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? pallet : null;
    }

    public async Task<PalletModel?> UpdateAsync(PalletModel pallet)
    {
        var palletFounded = await db.Pallets.FindAsync(pallet.Id);

        if (palletFounded is null)
        {
            await db.Pallets.AddAsync(mapper.Map<Pallet>(pallet));
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
        Pallet? palletFounded = await db.Pallets.FindAsync(id);

        if (palletFounded is null)
        {
            return false;
        }

        db.Pallets.Remove(palletFounded);
        var affected = await db.SaveChangesAsync();

        return affected == 1;
    }

    public async Task<int> CountAsync()
    {
        return await db.Pallets.CountAsync();
    }

    public async Task<bool?> AddBoxToPalletAsync(BoxModel box, int palletId)
    {
        var palletFounded = await db.Pallets.FindAsync(palletId);

        if (palletFounded is null)
        {
            return null;
        }

        var boxEf = await db.Boxes.FindAsync(box.Id);

        if (boxEf is null)
        {
            boxEf = mapper.Map<Box>(box);
            await db.Boxes.AddAsync(boxEf);
        }

        boxEf.PalletId = palletId;

        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool?> DeleteBoxFromPalletAsync(BoxModel box)
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
