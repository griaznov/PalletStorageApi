using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using DataContext;
using DataContext.Entities;

namespace PalletStorage.Tests.DataContext;

[Collection("StorageContextCollectionFixture")]
public class DataContextTests
{
    private readonly IStorageContext dbContext;
    private readonly string fileName;

    public DataContextTests(StorageContextFixture contextFixture)
    {
        fileName = contextFixture.FilePath;
        dbContext = contextFixture.DbContext;
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
        var box = new Box()
        {
            Width = 1,
            Length = 1,
            Height = 1,
            Weight = 1,
            ProductionDate = DateTime.Today,
            ExpirationDate = DateTime.Today,
        };

        // Act
        await dbContext.Boxes.AddAsync(box);
        await dbContext.SaveChangesAsync();

        // Assert
        dbContext.Boxes.Should().Contain(box);
    }

    [Fact(DisplayName = "3. Add a pallet model to the database")]
    protected async void AddPallet()
    {
        // Arrange
        var pallet = new Pallet()
        {
            Width = 1,
            Length = 1,
            Height = 1,
        };

        // Act
        await dbContext.Pallets.AddAsync(pallet);
        await dbContext.SaveChangesAsync();

        // Assert
        dbContext.Pallets.Should().Contain(pallet);
    }

    [Fact(DisplayName = "4. Add a pallet model containing box models to the database")]
    protected async void AddPalletWithBoxes()
    {
        // Arrange
        // Pallet - owner
        var pallet = new Pallet()
        {
            Width = 1,
            Length = 1,
            Height = 1,
            Id = 99
        };

        // Box with id ref to pallet-owner
        var box = new Box()
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
        await dbContext.Pallets.AddAsync(pallet);
        await dbContext.Boxes.AddAsync(box);
        await dbContext.SaveChangesAsync();

        // Assert
        dbContext.Pallets.Should().Contain(pallet);

        dbContext.Pallets
            .Include(p => p.Boxes)
            .First(p => p.Id == pallet.Id)
            .Boxes.Contains(box)
            .Should().BeTrue();
    }
}
