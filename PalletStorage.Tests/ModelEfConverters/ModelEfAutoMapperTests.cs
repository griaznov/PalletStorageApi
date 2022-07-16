using Xunit;
using AutoMapper;
using DataContext.Models.MappingProfiles;

namespace PalletStorage.Tests.ModelEfConverters;

public class ModelEfAutoMapperTests
{
    private readonly IMapper mapper;

    public ModelEfAutoMapperTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(MappingProfileEntity));
        });

        mapper = config.CreateMapper();
    }

    [Fact(DisplayName = "1. Test Mapping Profile for main models and EF models")]
    public void TestAutoMapperProfileEf()
    {
        // Assert
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
