using AutoMapper;
using DataContext;
using DataContext.Models.Converters;
using FluentAssertions;
using PalletStorage.Common.Models;
using PalletStorage.Repositories.Repositories;
using Xunit;

namespace PalletStorage.Tests.DataRepositories;
 
public class PalletRepositoryTests : IDisposable
{
    private readonly IStorageContext db;
    private readonly IPalletRepository palletRepo;

    public PalletRepositoryTests()
    {
        var fileName = FilesOperations.GenerateFileName("db");
        db = StorageContext.CreateContextAsync(fileName).GetAwaiter().GetResult();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(MappingProfileEf));
        });

        var mapper = config.CreateMapper();

        palletRepo = new PalletRepository(db, mapper);
    }

    [Fact(DisplayName = "1. Save common model of Pallet")]
    public async Task AddPalletAsync()
    {
        // Arrange
        const int width = 24;
        const int length = 82;
        const int height = 13;

        var pallet = Pallet.Create(width, length, height);

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

        var pallet = Pallet.Create(width, length, height);
        var box1 = Box.Create(2, 3, 4, 1, DateTime.Today, DateTime.Today);
        var box2 = Box.Create(2, 3, 3, 2, DateTime.Today, DateTime.Today);

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
        var pallet = new Pallet(1, 2, 3, 4, 981);
        var box = Box.Create(2, 3, 4, 1, DateTime.Today, DateTime.Today);

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
        var pallet = new Pallet(1, 2, 3, 4, 980);
        var box = new Box(1, 2, 3, 4, DateTime.Today, DateTime.Today, 101);

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
        var pallet = new Pallet(1, 2, 3, 4, 979);

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
        var pallet = new Pallet(1, 2, 3, 4, 970);
        await palletRepo.CreateAsync(pallet);

        // Act
        pallet = new Pallet(2, 3, 4, 5, 970);
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
        var pallet1 = Pallet.Create(1, 2, 3);
        var pallet2 = Pallet.Create(1, 2, 3);
        var pallet3 = Pallet.Create(1, 2, 3);
        var pallet4 = Pallet.Create(1, 2, 3);

        await palletRepo.CreateAsync(pallet1);
        await palletRepo.CreateAsync(pallet2);
        await palletRepo.CreateAsync(pallet3);
        await palletRepo.CreateAsync(pallet4);

        // Act
        var collection = await palletRepo.GetAllAsync(100, 1);

        // Assert
        collection.Should().HaveCount(3);
    }

    [Fact(DisplayName = "8. Retrieve Pallets with pagination - Take")]
    public async Task RetrieveWithTakeAsync()
    {
        // Arrange
        var pallet1 = Pallet.Create(1, 2, 3);
        var pallet2 = Pallet.Create(1, 2, 3);
        var pallet3 = Pallet.Create(1, 2, 3);
        var pallet4 = Pallet.Create(1, 2, 3);

        await palletRepo.CreateAsync(pallet1);
        await palletRepo.CreateAsync(pallet2);
        await palletRepo.CreateAsync(pallet3);
        await palletRepo.CreateAsync(pallet4);

        // Act
        var collection = await palletRepo.GetAllAsync(3);

        // Assert
        collection.Should().HaveCount(3);
    }

    public void Dispose()
    {
        db.Database.EnsureDeleted();
    }
}
