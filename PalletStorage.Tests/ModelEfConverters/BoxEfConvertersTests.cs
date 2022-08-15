using Xunit;
using AutoMapper;
using FluentAssertions;
using DataContext.Entities;
using PalletStorage.BusinessModels;
using PalletStorage.Repositories.Automapper;

namespace PalletStorage.Tests.ModelEfConverters;

public class BoxEfConvertersTests
{
    private readonly IMapper mapper;

    public BoxEfConvertersTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(BoxModelMappingProfile));
            cfg.AddProfile(typeof(PalletModelMappingProfile));
        });

        mapper = config.CreateMapper();
    }

    [Fact(DisplayName = "1. Box convert to BoxEfModel")]
    public void BoxConvertToBoxEfModel()
    {
        // Arrange
        var box = BoxModel.Create(2, 3, 4, 1, DateTime.Today, DateTime.Today);

        // Act
        Action action = () => { var boxModel = mapper.Map<Box>(box); };

        // Assert
        action.Should().NotThrow<Exception>();
    }

    [Fact(DisplayName = "2. BoxEfModel convert to Box")]
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

        // Act
        Action action = () => { var box = mapper.Map<BoxModel>(boxModel); };

        // Assert
        action.Should().NotThrow<Exception>();
    }
}
