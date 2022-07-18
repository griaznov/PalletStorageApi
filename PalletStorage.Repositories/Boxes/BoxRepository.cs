using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataContext;
using DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using PalletStorage.BusinessModels;

namespace PalletStorage.Repositories.Boxes;

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
        var boxEntity = await db.Boxes.FirstOrDefaultAsync(b => b.Id == id);

        return mapper.Map<BoxModel>(boxEntity);
    }

    public async Task<BoxModel?> CreateAsync(BoxModel box)
    {
        var boxEntity = mapper.Map<Box>(box);

        // add to database using EF Core
        await db.Boxes.AddAsync(boxEntity);

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? mapper.Map<BoxModel>(boxEntity) : null;
    }

    public async Task<BoxModel?> UpdateAsync(BoxModel box)
    {
        var boxEntity = await db.Boxes.FindAsync(box.Id);

        if (boxEntity is null)
        {
            boxEntity = mapper.Map<Box>(box);
            await db.Boxes.AddAsync(boxEntity);
        }
        else
        {
            // update in database new values for entry
            mapper.Map(box, boxEntity);
        }

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? mapper.Map<BoxModel>(boxEntity) : null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        // remove from database
        var boxFounded = await db.Boxes.FindAsync(id);

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
