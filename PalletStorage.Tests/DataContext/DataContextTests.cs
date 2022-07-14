using DataContext;
using DataContext.Models.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace PalletStorage.Tests.DataContext;

public class DataContextTests : IDisposable
{
    private readonly IStorageContext db;
    private readonly string fileName;

    public DataContextTests()
    {
        fileName = FilesOperations.GenerateFileName("db");
        db = StorageContext.CreateContextAsync(fileName).GetAwaiter().GetResult();
    }

    [Fact(DisplayName = "1. The database can be created by models")]
    protected void CreateDatabase()
    {
        // Assert
        File.Exists(fileName).Should().BeTrue("Because database must be created by models");
    }

    [Fact(DisplayName = "2. Add a box model to the database")]
    protected async void AddBox()
    {
        // Arrange
        var box = new BoxEfModel()
        {
            Width = 1,
            Length = 1,
            Height = 1,
            Weight = 1,
            ProductionDate = DateTime.Today,
            ExpirationDate = DateTime.Today,
        };

        // Act
        await db.Boxes.AddAsync(box);
        await db.SaveChangesAsync();

        // Assert
        db.Boxes.Should().HaveCount(1);
        db.Boxes.Should().Contain(box);
    }

    [Fact(DisplayName = "3. Add a pallet model to the database")]
    protected async void AddPallet()
    {
        // Arrange
        var pallet = new PalletEfModel()
        {
            Width = 1,
            Length = 1,
            Height = 1,
        };

        // Act
        await db.Pallets.AddAsync(pallet);
        await db.SaveChangesAsync();

        // Assert
        db.Pallets.Should().HaveCount(1);
        db.Pallets.Should().Contain(pallet);
    }

    [Fact(DisplayName = "4. Add a pallet model containing box models to the database")]
    protected async void AddPalletWithBoxes()
    {
        // Arrange
        // Pallet - owner
        var pallet = new PalletEfModel()
        {
            Width = 1,
            Length = 1,
            Height = 1,
            Id = 99
        };

        // Box with id ref to pallet-owner
        var box = new BoxEfModel()
        {
            Width = 1,
            Length = 1,
            Height = 1,
            Weight = 1,
            ProductionDate = DateTime.Today,
            ExpirationDate = DateTime.Today,
            PalletId = pallet.Id,
        };

        // Act
        await db.Pallets.AddAsync(pallet);
        await db.Boxes.AddAsync(box);
        await db.SaveChangesAsync();

        // Assert
        db.Pallets.Should().Contain(pallet);

        db.Pallets
            .Include(p => p.Boxes)
            .First(p => p.Id == pallet.Id)
            .Boxes.Contains(box)
            .Should().BeTrue();
    }

    public void Dispose()
    {
        db.Database.EnsureDeleted();
    }
}
