﻿using DataContext.Models.Converters;
using DataContext.Models.Models;
using FluentAssertions;
using PalletStorage.Common.CommonClasses;
using PalletStorage.WebApi.Models.Converters;
using PalletStorage.WebApi.Models.Models;
using Xunit;

namespace PalletStorage.Tests.ModelApiConverters;

public class BoxApiConvertersTests
{
    [Fact(DisplayName = "1. Box convert to BoxApiModel")]
    public void BoxConvertToBoxApiModel()
    {
        // Arrange
        var box = Box.Create(2, 3, 4, 1, DateTime.Today, DateTime.Today);

        // Act
        Action action = () => { var boxModel = box.ToApiModel(); };

        // Assert
        action.Should().NotThrow<Exception>();
    }

    [Fact(DisplayName = "2. BoxApiModel convert to Box")]
    public void BoxApiModelConvertToBox()
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

        // Act
        Action action = () => { var box = boxModel.ToCommonModel(); };

        // Assert
        action.Should().NotThrow<Exception>();
    }
}