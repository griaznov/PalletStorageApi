﻿using DataContext.Models.Converters;
using DataContext.Models.Models;
using FluentAssertions;
using PalletStorage.Common.CommonClasses;
using Xunit;

namespace PalletStorage.Tests.ModelEfConverters;

public class BoxApiConvertersTests
{
    [Fact(DisplayName = "1. Box convert to BoxEfModel")]
    public void BoxConvertToBoxEfModel()
    {
        // Arrange
        var box = Box.Create(2, 3, 4, 1, DateTime.Today, DateTime.Today);

        // Act
        Action action = () => { var boxModel = box.ToEfModel(); };

        // Assert
        action.Should().NotThrow<Exception>();
    }

    [Fact(DisplayName = "2. BoxEfModel convert to Box")]
    public void BoxEfModelConvertToBox()
    {
        // Arrange
        var boxModel = new BoxEfModel()
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