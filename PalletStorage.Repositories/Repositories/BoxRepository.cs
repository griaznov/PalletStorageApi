using DataContext;
using DataContext.Models.Converters;
using DataContext.Models.Models;
using Microsoft.EntityFrameworkCore;
using PalletStorage.Common.CommonClasses;

namespace PalletStorage.Repositories.Repositories;

public class BoxRepository : IBoxRepository
{
    // use an instance data context field because it should not be
    // cached due to their internal caching
    private readonly StorageDataContext db;

    public BoxRepository(StorageDataContext injectedContext)
    {
        db = injectedContext;
    }

    //public async Task<IEnumerable<BoxEfModel>> RetrieveAllAsync()
    public async Task<List<Box>> RetrieveAllAsync()
    {
        //return await Task.FromResult(db.Boxes.Select(box => box.ToCommonModel()).ToList());
        return await db.Boxes.Select(box => box.ToCommonModel()).ToListAsync();
    }

    public async Task<Box?> RetrieveAsync(int id)
    {
        var boxEf = await db.Boxes.FindAsync(id);

        return boxEf?.ToCommonModel();
    }

    public async Task<Box?> CreateAsync(Box box)
    {
        // add to database using EF Core
        await db.Boxes.AddAsync(box.ToEfModel());

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? box : null;
    }

    public async Task<Box?> UpdateAsync(int id, Box box)
    {
        BoxEfModel? boxFounded = await db.Boxes.FindAsync(id);

        if (boxFounded is null)
        {
            await db.Boxes.AddAsync(box.ToEfModel());
        }
        else
        {
            // update in database
            //db.Boxes.Update(box.ToEfModel());
            db.Entry(boxFounded).CurrentValues.SetValues(box.ToEfModel());
        }

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? box : null;
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        // remove from database
        BoxEfModel? boxFounded = await db.Boxes.FindAsync(id);

        if (boxFounded is null)
        {
            return null;
        }

        db.Boxes.Remove(boxFounded);
        var affected = await db.SaveChangesAsync();

        return affected == 1 ? true : null;
    }
}
