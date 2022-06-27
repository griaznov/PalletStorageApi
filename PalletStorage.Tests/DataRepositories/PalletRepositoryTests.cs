using DataContext;
using DataContext.Models.Converters;
using FluentAssertions;
using PalletStorage.Common.CommonClasses;
using PalletStorage.Repositories;
using PalletStorage.Repositories.Repositories;
using Xunit;

namespace PalletStorage.Tests.DataRepositories;
 
public class PalletRepositoryTests
{
    private readonly StorageDataContext db;
    private readonly IBoxRepository boxRepo;
    private readonly IPalletRepository palletRepo;

    public PalletRepositoryTests()
    {
        var fileName = FilesOperations.GenerateFileName("db");

        db = DataContextCreator.CreateDataContextAsync(fileName).Result;
        boxRepo = new BoxRepository(db);
        palletRepo = new PalletRepository(db);
    }

    [Fact(DisplayName = "1. Save common model of Pallet in database")]
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
    }

    [Fact(DisplayName = "2. Move common model of Box to Pallet in database")]
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
