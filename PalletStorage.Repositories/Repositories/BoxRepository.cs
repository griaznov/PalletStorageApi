using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataContext;
using DataContext.Models.Models;
using Microsoft.EntityFrameworkCore;
using PalletStorage.Common.Models;

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

    public async Task<List<Box>> GetAllAsync(int take, int skip = 0)
    {
        return await db.Boxes
            .Skip(skip)
            .Take(take)
            //.Select(box => mapper.Map<Box>(box))
            .ProjectTo<Box>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<Box?> GetAsync(int id)
    {
        var boxEf = await db.Boxes.FindAsync(id);

        return mapper.Map<Box>(boxEf);
    }

    public async Task<Box?> CreateAsync(Box box)
    {
        // add to database using EF Core
        await db.Boxes.AddAsync(mapper.Map<BoxEfModel>(box));

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? box : null;
    }

    public async Task<Box?> UpdateAsync(Box box)
    {
        BoxEfModel? boxFounded = await db.Boxes.FindAsync(box.Id);

        if (boxFounded is null)
        {
            await db.Boxes.AddAsync(mapper.Map<BoxEfModel>(box));
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
        BoxEfModel? boxFounded = await db.Boxes.FindAsync(id);

        if (boxFounded is null)
        {
            return false;
        }

        db.Boxes.Remove(boxFounded);
        var affected = await db.SaveChangesAsync();

        return affected == 1;
    }
}
