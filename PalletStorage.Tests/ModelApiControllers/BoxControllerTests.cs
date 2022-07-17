using Xunit;
using FluentAssertions;
using PalletStorage.WebApi.Controllers;
using PalletStorage.WebApi.Models.Models.Box;

namespace PalletStorage.Tests.ModelApiControllers;

[Collection("StorageContextCollectionFixture")]
public class BoxControllerTests
{
    private readonly BoxController controller;

    public BoxControllerTests(StorageContextFixture contextFixture)
    {
        controller = contextFixture.BoxController;
    }

    [Fact(DisplayName = "1. Create Box")]
    public async Task CreateBox()
    {
        // Arrange
        var box = new BoxCreateRequest()
        {
            Width = 1,
            Length = 1,
            Height = 1,
            Weight = 1,
            ProductionDate = DateTime.Today,
            ExpirationDate = DateTime.Today,
        };

        // Act
        var response = await controller.Create(box);

        // Assert
        response.Value.Should().NotBeNull().And.BeAssignableTo<BoxResponse>();
    }
}
