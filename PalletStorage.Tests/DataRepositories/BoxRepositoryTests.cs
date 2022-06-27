using DataContext;
using DataContext.Models.Converters;
using FluentAssertions;
using PalletStorage.Common.CommonClasses;
using PalletStorage.Repositories;
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

    [Fact(DisplayName = "1. Save common model of Box in database")]
    public async Task AddBoxAsync()
    {
        var date = new DateTime(2022, 7, 21, 19, 20, 17);
        const int width = 22;
        const int length = 31;
        const int height = 43;

        // Arrange
        var box = Box.Create(width, length, height, 11, date, date);

        //var fileName = FilesOperations.GenerateFileName("db");
        //await using StorageDataContext db = await DataContextCreator.CreateDataContextAsync(fileName);

        await boxRepo.CreateAsync(box);

        // Act
        //await db.AddBoxAsync(box);

        //Box boxConvertedFromDatabase = db.Boxes.First(b => b.Id == box.Id).ToCommonModel();

        var boxEfModel = db.Boxes.FirstOrDefault(boxes => boxes.ExpirationDate == date
                                                          && boxes.ProductionDate == date
                                                          && boxes.Width == width
                                                          && boxes.Length == length
                                                          && boxes.Height == height);
        if (boxEfModel == null)
        {
            throw new Exception();
        }

        var boxConvertedFromDatabase = boxEfModel.ToCommonModel();

        // Assert
        ObjectComparison.EqualsByJson(box, boxConvertedFromDatabase).Should().BeTrue();

    }


}
