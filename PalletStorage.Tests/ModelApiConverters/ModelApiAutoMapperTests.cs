﻿using Xunit;
using AutoMapper;
using PalletStorage.WebApi.Models.MappingProfiles;

namespace PalletStorage.Tests.ModelApiConverters;

public class ModelEfAutoMapperTests
{
    private readonly IMapper mapper;

    public ModelEfAutoMapperTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(MappingProfileApi));
        });

        mapper = config.CreateMapper();
    }

    [Fact(DisplayName = "1. Test Mapping Profile for main models and Api models")]
    public void TestAutoMapperProfileApi()
    {
        // Assert
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}