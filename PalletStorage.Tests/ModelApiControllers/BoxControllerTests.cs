using AutoMapper;
using DataContext;
using DataContext.Models.Converters;
using FluentAssertions;
using PalletStorage.Repositories.Repositories;
using PalletStorage.WebApi.Controllers;
using PalletStorage.WebApi.Models.Converters;
using PalletStorage.WebApi.Models.Models;
using PalletStorage.WebApi.Validators;
using Xunit;

namespace PalletStorage.Tests.ModelApiControllers;

public class BoxControllerTests : IDisposable
{
    private readonly IStorageContext db;
    private readonly BoxesController controller;

    public BoxControllerTests()
    {
        var fileName = FilesOperations.GenerateFileName("db");
        db = StorageContext.CreateContextAsync(fileName).GetAwaiter().GetResult();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(MappingProfileApi));
            cfg.AddProfile(typeof(MappingProfileEf));
        });

        var mapper = config.CreateMapper();

        var boxRepo = new BoxRepository(db, mapper);
        controller = new BoxesController(boxRepo, mapper, new BoxValidator());
    }

    [Fact(DisplayName = "1. Create Box")]
    public async Task CreateBox()
    {
        // Arrange
        var modelApi = new BoxApiModel()
        {
            Id = 30,
            Width = 1,
            Length = 1,
            Height = 1,
            Weight = 1,
            ProductionDate = DateTime.Today,
            ExpirationDate = DateTime.Today,
        };

        // Act
        var response = await controller.Create(modelApi);

        // Assert
        response.Value.Should().NotBeNull().And.BeAssignableTo<BoxApiModel>();
    }

    public void Dispose()
    {
        db.Database.EnsureDeleted();
    }
}
