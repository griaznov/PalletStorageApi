using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataContext;
using DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using PalletStorage.BusinessModels;

namespace PalletStorage.Repositories.Boxes;

public class BoxRepository : IBoxRepository
{
    private readonly IStorageContext dbContext;
    private readonly IMapper mapper;

    public BoxRepository(IStorageContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<List<BoxModel>> GetAllAsync(int take, int skip, CancellationToken token)
    {
        return await dbContext.Boxes
            .Skip(skip)
            .Take(take)
            .ProjectTo<BoxModel>(mapper.ConfigurationProvider)
            .ToListAsync(token);
    }

    public async Task<BoxModel?> GetAsync(int id, CancellationToken token)
    {
        var boxEntity = await dbContext.Boxes.FirstOrDefaultAsync(b => b.Id == id, token);

        return mapper.Map<BoxModel>(boxEntity);
    }

    public async Task<BoxModel?> CreateAsync(BoxModel box, CancellationToken token)
    {
        var boxEntity = mapper.Map<Box>(box);

        await dbContext.Boxes.AddAsync(boxEntity, token);

        var affected = await dbContext.SaveChangesAsync();

        return affected == 1 ? mapper.Map<BoxModel>(boxEntity) : null;
    }

    public async Task<BoxModel?> UpdateAsync(BoxModel box, CancellationToken token)
    {
        var boxEntity = await dbContext.Boxes.FirstOrDefaultAsync(b => b.Id == box.Id, token);

        if (boxEntity is null)
        {
            boxEntity = mapper.Map<Box>(box);
            await dbContext.Boxes.AddAsync(boxEntity, token);
        }
        else
        {
            // update in database new values for entry
            mapper.Map(box, boxEntity);
        }

        var affected = await dbContext.SaveChangesAsync();

        return affected == 1 ? mapper.Map<BoxModel>(boxEntity) : null;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken token)
    {
        var boxEntity = await dbContext.Boxes.FirstOrDefaultAsync(b => b.Id == id, token);

        if (boxEntity is null)
        {
            return false;
        }

        dbContext.Boxes.Remove(boxEntity);
 
        var affected = await dbContext.SaveChangesAsync();

        return affected == 1;
    }

    public async Task<int> CountAsync()
    {
        return await dbContext.Boxes.CountAsync();
    }
}
