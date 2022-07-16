using Xunit;
using FluentAssertions;
using AutoMapper;
using PalletStorage.Business.Models;
using PalletStorage.WebApi.Models.MappingProfiles;
using PalletStorage.WebApi.Models.Models;

namespace PalletStorage.Tests.ModelApiConverters;

public class PalletApiConvertersTests
{
    private readonly IMapper mapper;

    public PalletApiConvertersTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(MappingProfileApi));
        });

        mapper = config.CreateMapper();
    }

    [Fact(DisplayName = "1. Pallet with Boxes convert to PalletApiModel with BoxApiModel")]
    public void PalletConvertToPalletApiModel()
    {
        // Arrange
        var pallet = PalletModel.Create(2, 3, 4);
        var box = BoxModel.Create(2, 2, 2, 2, DateTime.Today);

        pallet.AddBox(box);

        // Act
        var palletModel = mapper.Map<PalletApiModel>(pallet);

        // Assert
        palletModel.Boxes.Should().HaveCount(1);
    }

    [Fact(DisplayName = "2. PalletApiModel with BoxApiModel convert to Pallet with Boxes")]
    public void PalletApiModelConvertToBox()
    {
        // Arrange
        var boxModel = new BoxApiModel()
        {
            Id = 33,
            Width = 1,
            Length = 1,
            Height = 1,
            Weight = 1,
            ProductionDate = DateTime.Today,
            ExpirationDate = DateTime.Today,
        };

        var palletModel = new PalletApiModel()
        {
            Id = 36,
            Width = 1,
            Length = 1,
            Height = 1,
            PalletWeight = 10,
            Boxes = new List<BoxApiModel> { boxModel }
        };

        // Act
        var pallet = mapper.Map<PalletModel>(palletModel);

        // Assert
        pallet.Boxes.Should().HaveCount(1);
    }
}
