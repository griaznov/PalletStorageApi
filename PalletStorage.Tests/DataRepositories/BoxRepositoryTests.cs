using AutoMapper;
using DataContext;
using DataContext.Models.Converters;
using FluentAssertions;
using PalletStorage.Common.Models;
using PalletStorage.Repositories.Repositories;
using Xunit;

namespace PalletStorage.Tests.DataRepositories;

public class BoxRepositoryTests : IDisposable
{
    private readonly IStorageContext db;
    private readonly IBoxRepository boxRepo;

    public BoxRepositoryTests()
    {
        var fileName = FilesOperations.GenerateFileName("db");
        db = StorageContext.CreateContextAsync(fileName).GetAwaiter().GetResult();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(MappingProfileEf));
        });

        var mapper = config.CreateMapper();

        boxRepo = new BoxRepository(db, mapper);
    }

    [Fact(DisplayName = "1. Save common model of Box")]
    public async Task AddBoxAsync()
    {
        // Arrange
        var date = new DateTime(2022, 7, 21, 19, 20, 17);
        const int width = 22;
        const int length = 31;
        const int height = 43;

        var box = Box.Create(width, length, height, 11, date, date);

        // Act
        await boxRepo.CreateAsync(box);

        // Assert
        var boxEfModel = db.Boxes.FirstOrDefault(boxes => boxes.ExpirationDate == date
                                                          && boxes.ProductionDate == date
                                                          && Math.Abs(boxes.Width - width) == 0 
                                                          && Math.Abs(boxes.Length - length) == 0
                                                          && Math.Abs(boxes.Height - height) == 0);
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
        var boxSaved = await boxRepo.GetAsync(box.Id);
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
        var box1 = Box.Create(1, 2, 3, 4, DateTime.Today, DateTime.Today);
        var box2 = Box.Create(1, 2, 3, 4, DateTime.Today, DateTime.Today);
        var box3 = Box.Create(1, 2, 3, 4, DateTime.Today, DateTime.Today);
        var box4 = Box.Create(1, 2, 3, 4, DateTime.Today, DateTime.Today);

        await boxRepo.CreateAsync(box1);
        await boxRepo.CreateAsync(box2);
        await boxRepo.CreateAsync(box3);
        await boxRepo.CreateAsync(box4);

        // Act
        var collection = await boxRepo.GetAllAsync(20, 1);

        // Assert
        collection.Should().HaveCount(3);
    }

    [Fact(DisplayName = "5. Retrieve Boxes with pagination - Take")]
    public async Task RetrieveWithTakeAsync()
    {
        // Arrange
        var box1 = Box.Create(1, 2, 3, 4, DateTime.Today, DateTime.Today);
        var box2 = Box.Create(1, 2, 3, 4, DateTime.Today, DateTime.Today);
        var box3 = Box.Create(1, 2, 3, 4, DateTime.Today, DateTime.Today);
        var box4 = Box.Create(1, 2, 3, 4, DateTime.Today, DateTime.Today);

        await boxRepo.CreateAsync(box1);
        await boxRepo.CreateAsync(box2);
        await boxRepo.CreateAsync(box3);
        await boxRepo.CreateAsync(box4);

        // Act
        var collection = await boxRepo.GetAllAsync(2);

        // Assert
        collection.Should().HaveCount(2);
    }

    public void Dispose()
    {
        db.Database.EnsureDeleted();
    }
}
