using Xunit;
using FluentAssertions;
using DataContext;
using PalletStorage.Business.Models;
using PalletStorage.Repositories;

namespace PalletStorage.Tests.DataRepositories;

[Collection("StorageContextCollectionFixture")]
public class BoxRepositoryTests
{
    private readonly IStorageContext db;
    private readonly IBoxRepository boxRepo;

    public BoxRepositoryTests(StorageContextFixture contextFixture)
    {
        db = contextFixture.Db;
        boxRepo = contextFixture.BoxRepo;
    }

    [Fact(DisplayName = "1. Save common model of Box")]
    public async Task AddBoxAsync()
    {
        // Arrange
        var date = new DateTime(2022, 7, 21, 19, 20, 17);
        const int width = 22;
        const int length = 31;
        const int height = 43;

        var box = BoxModel.Create(width, length, height, 11, date, date);

        // Act
        await boxRepo.CreateAsync(box);

        // Assert
        var boxEfModel = db.Boxes.FirstOrDefault(boxes => boxes.ExpirationDate == date
                                                          && boxes.ProductionDate == date
                                                          && (boxes.Width - width) == 0 
                                                          && (boxes.Length - length) == 0
                                                          && (boxes.Height - height) == 0);
        boxEfModel.Should().NotBeNull();
    }

    [Fact(DisplayName = "2. Delete common model Box")]
    public async Task DeleteBox()
    {
        // Arrange
        var box = new BoxModel(1, 2, 3, 4, DateTime.Today, DateTime.Today, 12);

        await boxRepo.CreateAsync(box);

        // Act
        await boxRepo.DeleteAsync(box.Id);

        // Assert
        var boxSaved = await boxRepo.GetAsync(box.Id);
        boxSaved?.Should().BeNull();
    }

    [Fact(DisplayName = "3. Update common model Box")]
    public async Task UpdateBox()
    {
        // Arrange
        var box = new BoxModel(1, 2, 3, 4, DateTime.Today, DateTime.Today, 13);
        await boxRepo.CreateAsync(box);

        // Act
        box = new BoxModel(2, 3, 4, 5, DateTime.Today, DateTime.Today, 13);
        await boxRepo.UpdateAsync(box);

        // Assert
        var boxSaved = await boxRepo.GetAsync(box.Id);

        boxSaved.Should().NotBeNull();

        boxSaved?.Width.Should().Be(2);
        boxSaved?.Length.Should().Be(3);
        boxSaved?.Height.Should().Be(4);
        boxSaved?.Weight.Should().Be(5);
    }

    [Fact(DisplayName = "4. Retrieve Boxes with pagination - Skip")]
    public async Task RetrieveWithSkipAsync()
    {
        // Arrange
        const int countToSkip = 3;
        await CreateTestCollectionAsync(4);
        var countNow = await boxRepo.CountAsync();

        // Act
        var collection = await boxRepo.GetAllAsync(countNow, countToSkip);

        // Assert
        collection.Should().HaveCount(countNow - countToSkip);
    }

    [Fact(DisplayName = "5. Retrieve Boxes with pagination - Take")]
    public async Task RetrieveWithTakeAsync()
    {
        // Arrange
        const int countMustBe = 4;

        await CreateTestCollectionAsync(countMustBe);

        // Act
        var collection = await boxRepo.GetAllAsync(countMustBe);

        // Assert
        collection.Should().HaveCount(countMustBe);
    }

    private async Task CreateTestCollectionAsync(int count)
    {
        for (var i = 0; i < count; i++)
        {
            await boxRepo.CreateAsync(BoxModel.Create(1, 2, 3, 4, DateTime.Today, DateTime.Today));
        }
    }
}
