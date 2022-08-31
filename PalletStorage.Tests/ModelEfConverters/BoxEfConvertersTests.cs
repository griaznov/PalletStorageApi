using Xunit;
using AutoMapper;
using FluentAssertions;
using DataContext.Entities;
using PalletStorage.BusinessModels;
using PalletStorage.Repositories.Automapper;
using PalletStorage.Tests.Infrastructure;

namespace PalletStorage.Tests.ModelEfConverters;

public class BoxEfConvertersTests
{
    private readonly IMapper mapper;
    private readonly DateTime dateTimeToday;

    public BoxEfConvertersTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(BoxModelMappingProfile));
            cfg.AddProfile(typeof(PalletModelMappingProfile));
        });

        mapper = config.CreateMapper();

        var dateTimeProvider = new DateTimeProvider();
        dateTimeToday = dateTimeProvider.GetToday();
    }

    [Fact(DisplayName = "1. Box convert to BoxEfModel")]
    public void BoxConvertToBoxEfModel()
    {
        // Arrange
        var box = BoxModel.Create(2, 3, 4, 1, dateTimeToday, dateTimeToday);

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
            ProductionDate = dateTimeToday,
            ExpirationDate = dateTimeToday,
        };

        // Act
        Action action = () => { var box = mapper.Map<BoxModel>(boxModel); };

        // Assert
        action.Should().NotThrow<Exception>();
    }
}
