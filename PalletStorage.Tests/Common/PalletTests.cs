﻿using PalletStorage.BusinessModels;
using PalletStorage.Tests.Infrastructure;
using Xunit;

namespace PalletStorage.Tests.Common;

public class PalletTests
{
    private readonly DateTime dateTimeToday = new DateTimeProvider().GetToday();

    [Fact(DisplayName = "1. Creating a normal pallet and checking the volume count")]
    public void CreationPallet()
    {
        // Act
        var pallet = PalletModel.Create(2, 3, 4);

        // Assert
        Assert.Equal(24, pallet.Volume);
    }

    [Fact(DisplayName = "2. Add box to pallet")]
    public void AddBoxToPallet()
    {
        // Arrange
        var pallet = PalletModel.Create(2, 3, 4);
        var box = BoxModel.Create(2, 2, 2, 2, dateTimeToday);

        // Act
        pallet.AddBox(box);

        // Assert
        Assert.Contains(box, pallet.Boxes);
    }

    [Fact(DisplayName = "3. Checking volume and weight calculation for pallet")]
    public void CheckingVolume()
    {
        // Arrange
        var pallet = PalletModel.Create(2, 3, 4);
        var box1 = BoxModel.Create(2, 2, 2, 2, dateTimeToday);
        var box2 = BoxModel.Create(2, 3, 2, 3, dateTimeToday);

        // Act
        pallet.AddBox(box1);
        pallet.AddBox(box2);

        // Assert
        Assert.Equal(24 + 8 + 12, pallet.Volume);
        Assert.Equal(30 + 2 + 3, pallet.Weight);
    }

    [Fact(DisplayName = "4. Checking empty expiration date for pallet")]
    public void CheckingDefaultExpirationDate()
    {
        var pallet = PalletModel.Create(2, 3, 4);

        // For an empty pallet, the date must be empty
        Assert.Equal(default, pallet.ExpirationDate);
    }

    [Fact(DisplayName = "5. Checking expiration date calculation for pallet")]
    public void CheckingExpirationDate()
    {
        // Arrange
        var lessDate = dateTimeToday;
        var biggerDate = lessDate.AddDays(1);
        var biggerDate2 = lessDate.AddDays(2);

        var pallet = PalletModel.Create(2, 3, 4);
        var box1 = BoxModel.Create(2, 2, 2, 2, dateTimeToday, lessDate);
        var box2 = BoxModel.Create(2, 3, 2, 3, dateTimeToday, biggerDate);
        var box3 = BoxModel.Create(2, 3, 2, 3, dateTimeToday, biggerDate2);

        // Act
        pallet.AddBox(box1);
        pallet.AddBox(box2);
        pallet.AddBox(box3);

        // Assert
        // Minimum value from boxes
        Assert.Equal(lessDate, pallet.ExpirationDate);
    }
}
