using Xunit;
using PalletStorage.Common.Models;

namespace PalletStorage.Tests.Common;

public class PalletTests
{
    [Fact(DisplayName = "1. Creating a normal pallet and checking the volume count")]
    public void CreationPallet()
    {
        // Act
        var pallet = Pallet.Create(2, 3, 4);

        // Assert
        Assert.Equal(24, pallet.Volume);
    }

    [Fact(DisplayName = "2. Add box to pallet")]
    public void AddBoxToPallet()
    {
        // Arrange
        var pallet = Pallet.Create(2, 3, 4);
        var box = Box.Create(2, 2, 2, 2, DateTime.Today);

        // Act
        pallet.AddBox(box);

        // Assert
        Assert.Contains(box, pallet.Boxes);
    }

    [Fact(DisplayName = "3. Checking volume and weight calculation for pallet")]
    public void CheckingVolume()
    {
        // Arrange
        var pallet = Pallet.Create(2, 3, 4);
        var box1 = Box.Create(2, 2, 2, 2, DateTime.Today);
        var box2 = Box.Create(2, 3, 2, 3, DateTime.Today);

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
        var pallet = Pallet.Create(2, 3, 4);

        // For an empty pallet, the date must be empty
        Assert.Equal(default, pallet.ExpirationDate);
    }

    [Fact(DisplayName = "5. Checking expiration date calculation for pallet")]
    public void CheckingExpirationDate()
    {
        // Arrange
        var lessDate = DateTime.Today;
        var biggerDate = lessDate.AddDays(1);
        var biggerDate2 = lessDate.AddDays(2);

        var pallet = Pallet.Create(2, 3, 4);
        var box1 = Box.Create(2, 2, 2, 2, DateTime.Today, lessDate);
        var box2 = Box.Create(2, 3, 2, 3, DateTime.Today, biggerDate);
        var box3 = Box.Create(2, 3, 2, 3, DateTime.Today, biggerDate2);

        // Act
        pallet.AddBox(box1);
        pallet.AddBox(box2);
        pallet.AddBox(box3);

        // Assert
        // Minimum value from boxes
        Assert.Equal(lessDate, pallet.ExpirationDate);
    }
}
