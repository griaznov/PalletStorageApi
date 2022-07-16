using Xunit;
using FluentAssertions;
using PalletStorage.WebApi.Controllers;
using PalletStorage.WebApi.Models.Models;

namespace PalletStorage.Tests.ModelApiControllers;

[Collection("StorageContextCollectionFixture")]
public class PalletControllerTests
{
    private readonly PalletController controller;

    public PalletControllerTests(StorageContextFixture contextFixture)
    {
        controller = contextFixture.PalletController;
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
}
