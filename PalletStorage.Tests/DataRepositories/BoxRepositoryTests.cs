using Xunit;
using FluentAssertions;
using DataContext;
using Microsoft.EntityFrameworkCore;
using PalletStorage.BusinessModels;
using PalletStorage.Repositories.Boxes;

namespace PalletStorage.Tests.DataRepositories;

[Collection("StorageContextCollectionFixture")]
public class BoxRepositoryTests
{
    private readonly IStorageContext dbContext;
    private readonly IBoxRepository boxRepo;
    private readonly DateTime dateTimeToday;

    public BoxRepositoryTests(StorageContextFixture contextFixture)
    {
        dbContext = contextFixture.DbContext;
        boxRepo = contextFixture.BoxRepo;

        var dateTimeProvider = new DateTimeProvider();
        dateTimeToday = dateTimeProvider.GetToday();
    }

    [Fact(DisplayName = "1. Save common model of Box")]
    public async Task AddBoxAsync()
    {
        // Arrange
        const int width = 22;
        const int length = 31;
        const int height = 43;

        var box = BoxModel.Create(width, length, height, 11, dateTimeToday, dateTimeToday);

        // Act
        var boxInRepo =  await boxRepo.CreateAsync(box);

        // Assert
        if (boxInRepo == null)
        {
            throw new ArgumentException($"The box was not created!");
        }

        var palletSaved = await dbContext.Boxes.FirstOrDefaultAsync(b => b.Id == boxInRepo.Id);

        palletSaved.Should().BeEquivalentTo(new { Length = length, Height = height, Width = width, boxInRepo.Id });
    }

    [Fact(DisplayName = "2. Delete common model Box")]
    public async Task DeleteBox()
    {
        // Arrange
        var box = new BoxModel(1, 2, 3, 4, dateTimeToday, dateTimeToday, 12);

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
        var box = new BoxModel(1, 2, 3, 4, dateTimeToday, dateTimeToday, 13);
        await boxRepo.CreateAsync(box);

        // Act
        box = new BoxModel(21, 35, 44, 15, dateTimeToday, dateTimeToday, 13);
        await boxRepo.UpdateAsync(box);

        // Assert
        var boxSaved = await boxRepo.GetAsync(box.Id);

        boxSaved.Should().NotBeNull();

        boxSaved?.Should().BeEquivalentTo(
            new { box.Length, box.Height, box.Width, box.Weight, box.Id },
            options => options.ExcludingMissingMembers());
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
        var collection = await boxRepo.GetAllAsync(countMustBe, 0);

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
