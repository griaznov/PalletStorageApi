using DataContext.Sqlite;
using EntityContext.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityContext.Repositories;

public class BoxCommonRepository : IBoxCommonRepository
{
    // use an instance data context field because it should not be
    // cached due to their internal caching
    private readonly StorageDataContext db;

    public BoxCommonRepository(StorageDataContext injectedContext)
    {
        db = injectedContext;
    }

    public async Task<IEnumerable<BoxEfModel>> RetrieveAllAsync()
    //public async Task<IList<BoxEfModel>> RetrieveAllAsync()
    {
        var listBoxes = await db.Boxes.ToListAsync();

        return listBoxes.AsEnumerable();
        //return listBoxes;
    }

    public async Task<BoxEfModel?> RetrieveAsync(string idString)
    {
        var id = new Guid(idString);

        return await db.Boxes.FindAsync(id);
    }

    public async Task<BoxEfModel?> CreateAsync(BoxEfModel b)
    {
        // add to database using EF Core
        EntityEntry<BoxEfModel> added = await db.Boxes.AddAsync(b);

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? b : null;
    }

    public async Task<BoxEfModel?> UpdateAsync(string idString, BoxEfModel b)
    {
        var id = new Guid(idString);

        // update in database
        db.Boxes.Update(b);

        var affected = await db.SaveChangesAsync();

        return affected == 1 ? b : null;
    }

    public async Task<bool?> DeleteAsync(string idString)
    {
        var id = new Guid(idString);

        // remove from database
        BoxEfModel? b = await db.Boxes.FindAsync(id);

        if (b is null)
        {
            return null;
        }

        db.Boxes.Remove(b);
        var affected = await db.SaveChangesAsync();

        return affected == 1 ? true : null;
    }

}
