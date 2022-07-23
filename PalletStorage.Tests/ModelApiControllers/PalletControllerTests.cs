using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        // Arrange
        var pallet = new PalletCreateRequest()
        {
            Width = 1,
            Length = 1,
            Height = 1,
        };

        // Fix a null reference exception trying to add the header to the response.
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

        // Act
        var response = (CreatedResult) await controller.Create(pallet, CancellationToken.None);

        // Assert
        response.StatusCode.Should().NotBeNull().And.Be(201);
        response.Value.Should().NotBeNull().And.BeAssignableTo<PalletResponse>();
    }
}
