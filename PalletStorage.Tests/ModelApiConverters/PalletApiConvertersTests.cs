using Xunit;
using FluentAssertions;
using AutoMapper;
using PalletStorage.BusinessModels;
using PalletStorage.Tests.Infrastructure;
using PalletStorage.WebApi.Automapper;
using PalletStorage.WebApi.Models.Box;
using PalletStorage.WebApi.Models.Pallet;

namespace PalletStorage.Tests.ModelApiConverters;

public class PalletApiConvertersTests
{
    private readonly IMapper mapper;
    private readonly DateTime dateTimeToday = new DateTimeProvider().GetToday();

    public PalletApiConvertersTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(PalletApiMappingProfile));
            cfg.AddProfile(typeof(BoxApiMappingProfile));
        });

        mapper = config.CreateMapper();
    }

    [Fact(DisplayName = "1. Pallet with Boxes convert to PalletApiModel with BoxApiModel")]
    public void PalletConvertToPalletApiModel()
    {
        // Arrange
        var pallet = PalletModel.Create(2, 3, 4);
        var box = BoxModel.Create(2, 2, 2, 2, dateTimeToday);

        pallet.AddBox(box);

        // Act
        var palletModel = mapper.Map<PalletResponse>(pallet);

        // Assert
        palletModel.Boxes.Should().HaveCount(1);
    }

    [Fact(DisplayName = "2. PalletApiModel with BoxApiModel convert to Pallet with Boxes")]
    public void PalletApiModelConvertToBox()
    {
        // Arrange
        var boxModel = new BoxResponse()
        {
            Id = 33,
            Width = 1,
            Length = 1,
            Height = 1,
            Weight = 1,
            ProductionDate = dateTimeToday,
            ExpirationDate = dateTimeToday,
        };

        var palletModel = new PalletResponse()
        {
            Id = 36,
            Width = 1,
            Length = 1,
            Height = 1,
            PalletWeight = 10,
            Boxes = new List<BoxResponse> { boxModel }
        };

        // Act
        var pallet = mapper.Map<PalletModel>(palletModel);

        // Assert
        pallet.Boxes.Should().HaveCount(1);
    }
}
