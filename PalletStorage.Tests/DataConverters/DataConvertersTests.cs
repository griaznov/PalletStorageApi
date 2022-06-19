using DataContext.Sqlite;
using EntityContext.Sqlite;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PalletStorage.Common.CommonClasses;
using PalletStorage.Common.Converters;
using PalletStorage.EfConverters;
using Xunit;
using static System.IO.Path;

namespace PalletStorage.Tests.DataConverters;

public class DataConvertersTests
{
    [Fact(DisplayName = "1. Save common model of Box in database")]
    public async Task StorageAddBoxAsync()
    {
        // Arrange
        var box = Box.Create(2, 3, 4, 1, DateTime.Today, DateTime.Today);

        var fileName = FilesOperations.GenerateFileName("db");
        await using StorageDataContext db = await DataContextCreator.CreateDataContextAsync(fileName);

        // Act
        await db.AddBoxAsync(box);

        Box boxConvertedFromDatabase = db.Boxes.First(b => b.Id == box.Id).ToCommonModel();

        // Assert
        ObjectComparison.EqualsByJson(box, boxConvertedFromDatabase).Should().BeTrue();

        //DeleteFile(fileName);
    }

    [Fact(DisplayName = "2. Save common model of Pallet in database")]
    public async Task StorageAddPalletAsync()
    {
        // Arrange
        var pallet = Pallet.Create(2, 3, 4);
        var box = Box.Create(2, 3, 4, 1, DateTime.Today, DateTime.Today);
        pallet.AddBox(box);

        var fileName = FilesOperations.GenerateFileName("db");
        await using StorageDataContext db = await DataContextCreator.CreateDataContextAsync(fileName);

        // Act
        await db.AddPalletAsync(pallet);

        Pallet palletConvertedFromDatabase = db.Pallets.First(b => b.Id == pallet.Id).ToCommonModel();

        // Assert
        ObjectComparison.EqualsByJson(pallet, palletConvertedFromDatabase).Should().BeTrue();

        //await db.DisposeAsync();
        //FilesOperations.DeleteFile(fileName);
    }

    [Fact(DisplayName = "3. Move common model of Box to Pallet in database")]
    public async Task StorageMoveBoxToPalletAsync()
    {
        // Arrange
        var pallet = Pallet.Create(2, 3, 4);
        var box = Box.Create(2, 3, 4, 1, DateTime.Today, DateTime.Today);

        var fileName = FilesOperations.GenerateFileName("db");
        await using StorageDataContext db = await DataContextCreator.CreateDataContextAsync(fileName);

        await db.AddBoxAsync(box);
        await db.AddPalletAsync(pallet);

        await db.MoveBoxToPalletAsync(box, pallet);

        //await db.DisposeAsync();

        //await using StorageDataContext db1 = await DataContextCreator.CreateDataContextAsync(fileName);

        // Act
        //await db1.MoveBoxToPalletAsync(box, pallet);

        //Pallet palletConvertedFromDatabase = db.Pallets.First(b => b.Id == pallet.Id).ToCommonModel();

        // Assert
        //ObjectComparison.EqualsByJson(pallet, palletConvertedFromDatabase).Should().BeTrue();

        //db.Pallets
        //    .Include(p => p.Boxes)
        //    .First(p => p.Id == pallet.Id)
        //    .Boxes.First(b => b.Id == box.Id)
        //    .Should()
        //    .NotBeNull();

        //DeleteFile(fileName);
    }

}
