using DataContext.Sqlite;
using EntityContext.Sqlite;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PalletStorage.Common;
using PalletStorage.Converters;
using Xunit;
using static System.IO.Path;

namespace PalletStorage.Tests.DataConverters;

public class DataConvertersTests
{
    [Fact(DisplayName = "1. Save common model of Box in database")]
    public async Task StorageAddBox()
    {
        // Arrange
        Box box = new(2, 3, 4, 1, DateTime.Today, DateTime.Today);

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
    public async Task StorageAddPallet()
    {
        // Arrange
        var pallet = Pallet.Create(2, 3, 4);

        var fileName = FilesOperations.GenerateFileName("db");
        await using StorageDataContext db = await DataContextCreator.CreateDataContextAsync(fileName);

        // Act
        await db.AddPalletAsync(pallet);

        Pallet palletConvertedFromDatabase = db.Pallets.First(b => b.Id == pallet.Id).ToCommonModel();

        // Assert
        ObjectComparison.EqualsByJson(pallet, palletConvertedFromDatabase).Should().BeTrue();

        //DeleteFile(fileName);
    }


}
