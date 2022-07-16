using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataContext;
using DataContext.Models.Entities;
using Microsoft.EntityFrameworkCore;
using PalletStorage.Business.Models;

namespace PalletStorage.Repositories.Repositories;

public class BoxRepository : IBoxRepository
{
    // use an instance data context field because it should not be
    // cached due to their internal caching
    private readonly IStorageContext db;
    private readonly IMapper mapper;

    public BoxRepository(IStorageContext storageContext, IMapper mapper)
    {
        db = storageContext;
        this.mapper = mapper;
    }

    public async Task<List<BoxModel>> GetAllAsync(int take, int skip = 0)
    {
        return await db.Boxes
            .Skip(skip)
            .Take(take)
            .ProjectTo<BoxModel>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<BoxModel?> GetAsync(int id)
    {
        var boxEf = await db.Boxes.FindAsync(id);

        return mapper.Map<BoxModel>(boxEf);
    }

    public async Task<BoxModel?> CreateAsync(BoxModel box)
    {
        // add to database using EF Core
        await db.Boxes.AddAsync(mapper.Map<Box>(box));

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? box : null;
    }

    public async Task<BoxModel?> UpdateAsync(BoxModel box)
    {
        Box? boxFounded = await db.Boxes.FindAsync(box.Id);

        if (boxFounded is null)
        {
            await db.Boxes.AddAsync(mapper.Map<Box>(box));
        }
        else
        {
            // update in database new values for entry
            mapper.Map(box, boxFounded);
        }

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? box : null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        // remove from database
        Box? boxFounded = await db.Boxes.FindAsync(id);

        if (boxFounded is null)
        {
            return false;
        }

        db.Boxes.Remove(boxFounded);
        var affected = await db.SaveChangesAsync();

        return affected == 1;
    }

    public async Task<int> CountAsync()
    {
        return await db.Boxes.CountAsync();
    }
}
