using EntityContext.Sqlite;
using PalletStorage.Common;
using DataContext.Sqlite;

namespace PalletStorage.Converters;

public static class StorageConverter
{
    public static async Task<int> AddPalletAsync(this StorageDataContext db, Pallet pallet)
    {
        await db.Pallets.AddAsync(pallet.ToEfModel());

        return await db.SaveChangesAsync();
    }

    public static async Task<int> AddBoxAsync(this StorageDataContext db, Box box)
    {
        await db.Boxes.AddAsync(box.ToEfModel());

        return await db.SaveChangesAsync();
    }

    public static async Task<int> MoveBoxToPalletAsync(this StorageDataContext db, Box box, Pallet pallet)
    {
        if (pallet.Boxes.Contains(box))
        {
            return 0;
        }

        pallet.AddBox(box);

        db.Pallets.Update(pallet.ToEfModel());
        db.Boxes.Update(box.ToEfModel());

        //db.Update(box.ToEfModel());
        //db.Update(pallet.ToEfModel());
        //db.Remove()

        return await db.SaveChangesAsync();
    }


}
