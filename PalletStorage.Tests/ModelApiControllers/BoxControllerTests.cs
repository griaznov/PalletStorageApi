using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PalletStorage.WebApi.Controllers;
using PalletStorage.WebApi.Models.Box;

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

        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

        // Act
        var response = (CreatedResult) await controller.Create(box, CancellationToken.None);

        // Assert
        response.StatusCode.Should().NotBeNull().And.Be(201);
        response.Value.Should().NotBeNull().And.BeAssignableTo<BoxResponse>();
    }
}
