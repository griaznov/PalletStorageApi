using Xunit;
using FluentAssertions;
using PalletStorage.WebApi.Controllers;
using PalletStorage.WebApi.Models.Pallet;

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
        var pallet = new PalletCreateRequest()
        {
            Width = 1,
            Length = 1,
            Height = 1,
        };

        // Act
        var response = await controller.Create(pallet);

        // Assert
        response.Value.Should().NotBeNull().And.BeAssignableTo<PalletResponse>();
    }
}
