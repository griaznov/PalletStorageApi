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

public class PalletControllerTests : IDisposable
{
    private readonly StorageDataContext db;
    private readonly PalletController controller;

    public PalletControllerTests()
    {
        var fileName = FilesOperations.GenerateFileName("db");
        db = DataContextCreator.CreateDataContextAsync(fileName).Result;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(MappingProfileApi));
            cfg.AddProfile(typeof(MappingProfileEf));
        });

        var mapper = config.CreateMapper();

        var repo = new PalletRepository(db, mapper);
        controller = new PalletController(repo, mapper, new PalletValidator());
    }

    [Fact(DisplayName = "1. Create Pallet")]
    public async Task CreateBox()
    {
        var palletModel = new PalletApiModel()
        {
            Id = 113,
            Width = 1,
            Length = 1,
            Height = 1,
            PalletWeight = 10,
        };

        // Act
        var response = await controller.Create(palletModel);

        // Assert
        response.Value.Should().NotBeNull().And.BeAssignableTo<PalletApiModel>();
    }

    public void Dispose()
    {
        db.Database.EnsureDeleted();
    }
}
