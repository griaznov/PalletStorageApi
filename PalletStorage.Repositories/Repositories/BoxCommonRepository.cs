using DataContext.Sqlite;
using EntityContext.Models.Converters;
using EntityContext.Models.Models;
using Microsoft.EntityFrameworkCore;
using PalletStorage.Common.CommonClasses;

namespace PalletStorage.Repositories.Repositories;

public class BoxCommonRepository : IBoxCommonRepository
{
    // use an instance data context field because it should not be
    // cached due to their internal caching
    private readonly StorageDataContext db;

    public BoxCommonRepository(StorageDataContext injectedContext)
    {
        db = injectedContext;
    }

    //public async Task<IEnumerable<BoxEfModel>> RetrieveAllAsync()
    public async Task<List<Box>> RetrieveAllAsync()
    {
        //return await Task.FromResult(db.Boxes.Select(box => box.ToCommonModel()).ToList());
        return await db.Boxes.Select(box => box.ToCommonModel()).ToListAsync();
    }

    public async Task<Box?> RetrieveAsync(string id)
    {
        var boxEfModel = await db.Boxes.FindAsync(new Guid(id));

        return boxEfModel?.ToCommonModel();
    }

    public async Task<Box?> CreateAsync(Box box)
    {
        // add to database using EF Core
        await db.Boxes.AddAsync(box.ToEfModel());

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? box : null;
    }

    public async Task<Box?> UpdateAsync(string idString, Box box)
    {
        var id = new Guid(idString);

        // update in database
        db.Boxes.Update(box.ToEfModel());

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? box : null;
    }

    public async Task<bool?> DeleteAsync(string id)
    {
        // remove from database
        BoxEfModel? box = await db.Boxes.FindAsync(new Guid(id));

        if (box is null)
        {
            return null;
        }

        db.Boxes.Remove(box);

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? true : null;
    }

}
