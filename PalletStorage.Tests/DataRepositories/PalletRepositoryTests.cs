using Xunit;
using FluentAssertions;
using DataContext;
using PalletStorage.Business.Models;
using PalletStorage.Repositories.Repositories;

namespace PalletStorage.Tests.DataRepositories;

[Collection("StorageContextCollectionFixture")]
public class PalletRepositoryTests
{
    private readonly IStorageContext db;
    private readonly IPalletRepository palletRepo;

    public PalletRepositoryTests(StorageContextFixture contextFixture)
    {
        db = contextFixture.Db;
        palletRepo = contextFixture.PalletRepo;
    }

    [Fact(DisplayName = "1. Save common model of Pallet")]
    public async Task AddPalletAsync()
    {
        // Arrange
        const int width = 24;
        const int length = 82;
        const int height = 13;

        var pallet = PalletModel.Create(width, length, height);

        // Act
        await palletRepo.CreateAsync(pallet);

        // Assert
        var palletSaved = db.Pallets.FirstOrDefault(p => p.Width == width
                                                         && p.Length == length
                                                         && p.Height == height);
        palletSaved.Should().NotBeNull();
    }

    [Fact(DisplayName = "2. Save common model of Pallet with Boxes")]
    public async Task AddPalletWithBoxesAsync()
    {
        // Arrange
        const int width = 21;
        const int length = 32;
        const int height = 53;

        var pallet = PalletModel.Create(width, length, height);
        var box1 = BoxModel.Create(2, 3, 4, 1, DateTime.Today, DateTime.Today);
        var box2 = BoxModel.Create(2, 3, 3, 2, DateTime.Today, DateTime.Today);

        pallet.AddBox(box1);
        pallet.AddBox(box2);

        // Act
        await palletRepo.CreateAsync(pallet);

        // Assert
        var palletSaved = db.Pallets.FirstOrDefault(p => p.Width == width
                                                         && p.Length == length
                                                         && p.Height == height);
        palletSaved.Should().NotBeNull();
        palletSaved?.Boxes.Should().HaveCount(2);
    }

    [Fact(DisplayName = "3. Add Box to common model Pallet")]
    public async Task AddBoxToPalletAsync()
    {
        // Arrange
        var pallet = new PalletModel(1, 2, 3, 981);
        var box = BoxModel.Create(2, 3, 4, 1, DateTime.Today, DateTime.Today);

        await palletRepo.CreateAsync(pallet);

        // Act
        await palletRepo.AddBoxToPalletAsync(box, pallet.Id);

        // Assert
        var palletSaved = await palletRepo.GetAsync(pallet.Id);

        palletSaved.Should().NotBeNull();
        palletSaved?.Boxes.Should().HaveCount(1);
    }

    [Fact(DisplayName = "4. Delete Box from common model Pallet")]
    public async Task DeleteBoxFromPallet()
    {
        // Arrange
        var pallet = new PalletModel(1, 2, 3, 980);
        var box = new BoxModel(1, 2, 3, 4, DateTime.Today, DateTime.Today, 101);

        pallet.AddBox(box);
        await palletRepo.CreateAsync(pallet);

        // Act
        await palletRepo.DeleteBoxFromPalletAsync(box);

        // Assert
        var palletSaved = await palletRepo.GetAsync(pallet.Id);

        palletSaved.Should().NotBeNull();
        palletSaved?.Boxes.Should().HaveCount(0);
    }

    [Fact(DisplayName = "5. Delete common model Pallet")]
    public async Task DeletePallet()
    {
        // Arrange
        var pallet = new PalletModel(1, 2, 3, 979);

        await palletRepo.CreateAsync(pallet);

        // Act
        await palletRepo.DeleteAsync(pallet.Id);

        // Assert
        var palletSaved = await palletRepo.GetAsync(pallet.Id);
        palletSaved?.Should().BeNull();
    }

    [Fact(DisplayName = "6. Update common model Pallet")]
    public async Task UpdatePallet()
    {
        // Arrange
        var pallet = new PalletModel(1, 2, 3, 970);
        await palletRepo.CreateAsync(pallet);

        // Act
        pallet = new PalletModel(2, 3, 4, 970);
        await palletRepo.UpdateAsync(pallet);

        // Assert
        var palletSaved = await palletRepo.GetAsync(pallet.Id);

        palletSaved.Should().NotBeNull();

        palletSaved?.Width.Should().Be(2);
        palletSaved?.Length.Should().Be(3);
        palletSaved?.Height.Should().Be(4);
    }

    [Fact(DisplayName = "7. Retrieve Pallets with pagination - Skip")]
    public async Task RetrieveWithSkipAsync()
    {
        // Arrange
        const int countToSkip = 3;
        await CreateTestCollectionAsync(4);
        var countNow = await palletRepo.CountAsync();

        // Act
        var collection = await palletRepo.GetAllAsync(countNow, countToSkip);

        // Assert
        collection.Should().HaveCount(countNow - countToSkip);
    }

    [Fact(DisplayName = "8. Retrieve Pallets with pagination - Take")]
    public async Task RetrieveWithTakeAsync()
    {
        // Arrange
        const int countMustBe = 4;

        await CreateTestCollectionAsync(countMustBe);

        // Act
        var collection = await palletRepo.GetAllAsync(countMustBe);

        // Assert
        collection.Should().HaveCount(countMustBe);
    }

    private async Task CreateTestCollectionAsync(int count)
    {
        for (var i = 0; i < count; i++)
        {
            await palletRepo.CreateAsync(PalletModel.Create(1, 2, 3));
        }
    }
}
