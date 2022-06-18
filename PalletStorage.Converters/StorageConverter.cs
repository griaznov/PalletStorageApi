using EntityContext.Sqlite;
using PalletStorage.Common;
using DataContext.Sqlite;

namespace PalletStorage.Converters;

public static class StorageConverter
{
    public static async Task AddPalletAsync(this StorageDataContext db, Pallet pallet)
    {
        await db.Pallets.AddAsync(pallet.ToEfModel());
        await db.SaveChangesAsync();
    }

    public static async Task AddBoxAsync(this StorageDataContext db, Box box)
    {
        await db.Boxes.AddAsync(box.ToEfModel());
        await db.SaveChangesAsync();
    }

    public static async Task MoveBoxToPalletAsync(this StorageDataContext db, Box box, Pallet pallet)
    {
        if (!pallet.Boxes.Contains(box))
        {
            pallet.AddBox(box);
        }

        db.Pallets.Update(pallet.ToEfModel());
        await db.SaveChangesAsync();
    }


}
