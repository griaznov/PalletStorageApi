using FluentAssertions;
using PalletStorage.BusinessModels;
using PalletStorage.Tests.Infrastructure;
using Xunit;

namespace PalletStorage.Tests.Common;

public class BoxTests
{
    private readonly DateTime dateTimeToday = new DateTimeProvider().GetToday();

    [Fact(DisplayName = "1. Creating a normal box and checking the volume count")]
    public void CreationBox()
    {
        // Act
        BoxModel testBox = new(2, 3, 4, 1, dateTimeToday, dateTimeToday);

        // Assert
        Assert.Equal(24, testBox.Volume);
    }

    [Fact(DisplayName = "1.1 Creating a normal box and checking the volume count by Create()")]
    public void CreationBoxByCreate()
    {
        // Act
        var testBox = BoxModel.Create(2, 3, 4, 1, dateTimeToday, dateTimeToday);

        // Assert
        Assert.Equal(24, testBox.Volume);
    }

    [Fact(DisplayName = "2. Creating a box without dates")]
    public void CreationWithoutDates()
    {
        // Act
        Action action = () => { var box = new BoxModel(2, 3, 4, 1); };

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>("Creating a box without dates must trow Exception!");
    }

    [Fact(DisplayName = "3. Creating a box without expiration date")]
    public void CreationWithoutExpirationDate()
    {
        // Arrange
        var todayDate = dateTimeToday;

        // Act
        BoxModel testBox = new(2, 3, 4, 1, todayDate);

        // Assert
        Assert.Equal(testBox.ExpirationDate, todayDate.AddDays(100));
    }
}
