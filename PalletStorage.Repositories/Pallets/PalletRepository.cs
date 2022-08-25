using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataContext;
using DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using PalletStorage.BusinessModels;

namespace PalletStorage.Repositories.Pallets;

public class PalletRepository : IPalletRepository
{
    private readonly IStorageContext dbContext;
    private readonly IMapper mapper;

    public PalletRepository(IStorageContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<IReadOnlyCollection<PalletModel>> GetAllAsync(int take, int skip, CancellationToken token)
    {
        return await dbContext.Pallets
            .Skip(skip)
            .Take(take)
            .Include(p => p.Boxes)
            .ProjectTo<PalletModel>(mapper.ConfigurationProvider)
            .ToListAsync(token);
    }

    public async Task<PalletModel?> GetAsync(int id, CancellationToken token)
    {
        var palletEntity = await dbContext.Pallets.FirstOrDefaultAsync(p => p.Id == id, token);

        return mapper.Map<PalletModel>(palletEntity);
    }

    public async Task<PalletModel?> CreateAsync(PalletModel pallet, CancellationToken token)
    {
        var palletEntity = mapper.Map<Pallet>(pallet);

        await dbContext.Pallets.AddAsync(palletEntity, token);

        var affected = await dbContext.SaveChangesAsync();

        return affected > 0 ? mapper.Map<PalletModel>(palletEntity) : null;
    }

    public async Task<PalletModel?> UpdateAsync(PalletModel pallet, CancellationToken token)
    {
        var palletEntity = await dbContext.Pallets.FirstOrDefaultAsync(p => p.Id == pallet.Id, token);

        if (palletEntity is null)
        {
            throw new ArgumentException($"The pallet with id {pallet.Id} was not found for updating!");
        }

        // update in database new values for entry
        mapper.Map(pallet, palletEntity);

        var affected = await dbContext.SaveChangesAsync();

        return affected > 0 ? mapper.Map<PalletModel>(palletEntity) : null;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken token)
    {
        var palletEntity = await dbContext.Pallets.FirstOrDefaultAsync(p => p.Id == id, token);

        if (palletEntity is null)
        {
            return false;
        }

        dbContext.Pallets.Remove(palletEntity);
        var affected = await dbContext.SaveChangesAsync();

        return affected == 1;
    }

    public async Task<int> CountAsync()
    {
        return await dbContext.Pallets.CountAsync();
    }

    public async Task<bool?> AddBoxToPalletAsync(BoxModel box, int palletId, CancellationToken token)
    {
        var palletEntity = await dbContext.Pallets.FirstOrDefaultAsync(p => p.Id == palletId, token);

        if (palletEntity is null)
        {
            return null;
        }

        var boxEf = await dbContext.Boxes.FindAsync(box.Id);

        if (boxEf is null)
        {
            boxEf = mapper.Map<Box>(box);
            await dbContext.Boxes.AddAsync(boxEf, token);
        }

        boxEf.PalletId = palletId;

        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool?> DeleteBoxFromPalletAsync(BoxModel box, CancellationToken token)
    {
        var boxEntity = await dbContext.Boxes.FirstOrDefaultAsync(b => b.Id == box.Id, token);

        if (boxEntity == null)
        {
            return null;
        }

        boxEntity.PalletId = null;

        return await dbContext.SaveChangesAsync() > 0;
    }
}
