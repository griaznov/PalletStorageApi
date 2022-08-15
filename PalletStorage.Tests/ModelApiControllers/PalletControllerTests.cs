using AutoMapper;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PalletStorage.BusinessModels;
using PalletStorage.Repositories.Pallets;
using PalletStorage.WebApi.Controllers;
using PalletStorage.WebApi.Models.Pallet;

namespace PalletStorage.Tests.ModelApiControllers;

[Collection("StorageContextCollectionFixture")]
public class PalletControllerTests
{
    private readonly PalletController controller;
    private readonly IMapper mapper;

    public PalletControllerTests(StorageContextFixture contextFixture)
    {
        controller = contextFixture.PalletController;
        mapper = contextFixture.Mapper;
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

    [Fact(DisplayName = "2. Get Pallet with mocking repo")]
    public async Task GetPallet()
    {
        // Arrange
        var token = CancellationToken.None;
        const int idTestPallet = 1;

        var mockPalletRepo = new Mock<IPalletRepository>();

        mockPalletRepo.Setup(repo => repo.GetAsync(idTestPallet, token))
            .ReturnsAsync(GetTestPalletCollection().FirstOrDefault(p => p.Id == idTestPallet));

        var emulatedController = new PalletController(mockPalletRepo.Object, mapper) {
            // Fix a null reference exception trying to add the header to the response.
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() } };

        // Act
        var response = (ObjectResult)await emulatedController.GetPallet(idTestPallet, token);

        // Assert
        response.StatusCode.Should().NotBeNull().And.Be(200);
        response.Value.Should().NotBeNull().And.BeAssignableTo<PalletResponse>();
    }

    private static IEnumerable<PalletModel> GetTestPalletCollection() =>
        new List<PalletModel>()
        {
            new(1, 2, 3, 1),
            new(2, 2, 3, 2),
            new(3, 2, 3, 3),
            new(4, 2, 3, 4)
        };
}
