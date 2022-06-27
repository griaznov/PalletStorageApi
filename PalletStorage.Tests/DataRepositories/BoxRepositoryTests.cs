using DataContext;
using FluentAssertions;
using PalletStorage.Common.CommonClasses;
using PalletStorage.Repositories.Repositories;
using Xunit;

namespace PalletStorage.Tests.DataRepositories;

public class BoxRepositoryTests
{
    private readonly StorageDataContext db;
    private readonly IBoxRepository boxRepo;

    public BoxRepositoryTests()
    {
        var fileName = FilesOperations.GenerateFileName("db");

        db = DataContextCreator.CreateDataContextAsync(fileName).Result;
        boxRepo = new BoxRepository(db);
    }

    [Fact(DisplayName = "1. Save common model of Box")]
    public async Task AddBoxAsync()
    {
        var date = new DateTime(2022, 7, 21, 19, 20, 17);
        const int width = 22;
        const int length = 31;
        const int height = 43;

        var box = Box.Create(width, length, height, 11, date, date);

        // Arrange
        await boxRepo.CreateAsync(box);

        // Assert
        var boxEfModel = db.Boxes.FirstOrDefault(boxes => boxes.ExpirationDate == date
                                                          && boxes.ProductionDate == date
                                                          && boxes.Width == width
                                                          && boxes.Length == length
                                                          && boxes.Height == height);
        boxEfModel.Should().NotBeNull();
    }

    [Fact(DisplayName = "2. Delete common model Box")]
    public async Task DeleteBox()
    {
        // Arrange
        var box = new Box(1, 2, 3, 4, DateTime.Today, DateTime.Today, 12);

        await boxRepo.CreateAsync(box);

        // Act
        await boxRepo.DeleteAsync(box.Id);

        // Assert
        var boxSaved = await boxRepo.RetrieveAsync(box.Id);
        boxSaved?.Should().BeNull();
    }

    [Fact(DisplayName = "3. Update common model Box")]
    public async Task UpdateBox()
    {
        // Arrange
        var box = new Box(1, 2, 3, 4, DateTime.Today, DateTime.Today, 13);
        await boxRepo.CreateAsync(box);

        // Act
        box = new Box(2, 3, 4, 5, DateTime.Today, DateTime.Today, 13);
        await boxRepo.UpdateAsync(box);

        // Assert
        var boxSaved = await boxRepo.RetrieveAsync(box.Id);

        boxSaved.Should().NotBeNull();

        boxSaved?.Width.Should().Be(2);
        boxSaved?.Length.Should().Be(3);
        boxSaved?.Height.Should().Be(4);
        boxSaved?.Weight.Should().Be(5);
    }
}
