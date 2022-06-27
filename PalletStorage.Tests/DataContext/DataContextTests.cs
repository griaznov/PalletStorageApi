using DataContext;
using DataContext.Models.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace PalletStorage.Tests.DataContext;

public class DataContextTests
{
    [Fact(DisplayName = "1. The database can be created by models")]
    public async Task CreateDatabase()
    {
        // Arrange
        var fileName = FilesOperations.GenerateFileName("db");

        // Act
        await using StorageDataContext dbTest = await DataContextCreator.CreateDataContextAsync(fileName);

        // Assert
        File.Exists(fileName).Should().BeTrue("Because database must be created by models");

        //FilesOperations.DeleteFile(fileName);
    }

    [Fact(DisplayName = "2. Add a box model to the database")]
    public async void AddBox()
    {
        // Arrange
        var fileName = FilesOperations.GenerateFileName("db");

        await using StorageDataContext db = await DataContextCreator.CreateDataContextAsync(fileName);

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

        //DeleteFile(fileName);
    }

    [Fact(DisplayName = "3. Add a pallet model to the database")]
    public async void AddPallet()
    {
        // Arrange
        var fileName = FilesOperations.GenerateFileName("db");

        await using StorageDataContext db = await DataContextCreator.CreateDataContextAsync(fileName);

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

        //DeleteFile(fileName);
    }

    [Fact(DisplayName = "4. Add a pallet model containing box models to the database")]
    public async void AddPalletWithBoxes()
    {
        // Arrange
        var fileName = FilesOperations.GenerateFileName("db");

        await using StorageDataContext db = await DataContextCreator.CreateDataContextAsync(fileName);

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

        //DeleteFile(fileName);
    }
}
