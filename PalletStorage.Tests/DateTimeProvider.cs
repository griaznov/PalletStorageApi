namespace PalletStorage.Tests;

internal class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetToday() => DateTime.Today;
}
