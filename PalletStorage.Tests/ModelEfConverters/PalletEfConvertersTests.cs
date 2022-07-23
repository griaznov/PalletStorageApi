using Xunit;
using FluentAssertions;
using AutoMapper;
using DataContext.Entities;
using PalletStorage.BusinessModels;
using PalletStorage.Repositories.Automapper;

namespace PalletStorage.Tests.ModelEfConverters;

public class PalletEfConvertersTests
{
    private readonly IMapper mapper;

    public PalletEfConvertersTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(BoxModelMappingProfile));
            cfg.AddProfile(typeof(PalletModelMappingProfile));
        });

        mapper = config.CreateMapper();
    }

    [Fact(DisplayName = "1. Pallet with Boxes convert to PalletEfModel with BoxesEfModel")]
    public void PalletConvertToBoxEfModel()
    {
        // Arrange
        var pallet = PalletModel.Create(2, 3, 4);
        var box = BoxModel.Create(2, 2, 2, 2, DateTime.Today);

        pallet.AddBox(box);

        // Act
        var palletModel = mapper.Map<Pallet>(pallet);

        // Assert
        palletModel.Boxes.Should().HaveCount(1);
    }

    [Fact(DisplayName = "2. PalletEfModel with BoxesEfModel convert to Pallet with Boxes")]
    public void BoxEfModelConvertToBox()
    {
        // Arrange
        var boxModel = new Box()
        {
            Id = 33,
            Width = 1,
            Length = 1,
            Height = 1,
            Weight = 1,
            ProductionDate = DateTime.Today,
            ExpirationDate = DateTime.Today,
        };

        var palletModel = new Pallet()
        {
            Id = 36,
            Width = 1,
            Length = 1,
            Height = 1,
            PalletWeight = 10,
            Boxes = new List<Box> { boxModel }
        };

        // Act
        var pallet = mapper.Map<PalletModel>(palletModel);

        // Assert
        pallet.Boxes.Should().HaveCount(1);
    }
}
