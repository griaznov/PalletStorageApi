using Xunit;
using AutoMapper;
using FluentAssertions;
using PalletStorage.BusinessModels;
using PalletStorage.Tests.Infrastructure;
using PalletStorage.WebApi.Automapper;
using PalletStorage.WebApi.Models.Box;

namespace PalletStorage.Tests.ModelApiConverters;

public class BoxApiConvertersTests
{
    private readonly IMapper mapper;
    private readonly DateTime dateTimeToday;

    public BoxApiConvertersTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(BoxApiMappingProfile));
        });

        mapper = config.CreateMapper();

        var dateTimeProvider = new DateTimeProvider();
        dateTimeToday = dateTimeProvider.GetToday();
    }

    [Fact(DisplayName = "1. Box convert to BoxApiModel")]
    public void BoxConvertToBoxApiModel()
    {
        // Arrange
        var box = BoxModel.Create(2, 3, 4, 1, dateTimeToday, dateTimeToday);

        // Act
        Action action = () => { var boxModel = mapper.Map<BoxResponse>(box); };

        // Assert
        action.Should().NotThrow<Exception>();
    }

    [Fact(DisplayName = "2. BoxApiModel convert to Box")]
    public void BoxApiModelConvertToBox()
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

        // Act
        Action action = () => { var box = mapper.Map<BoxModel>(boxModel); };

        // Assert
        action.Should().NotThrow<Exception>();
    }
}
