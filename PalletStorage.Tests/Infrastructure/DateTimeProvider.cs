namespace PalletStorage.Tests.Infrastructure;

internal class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetToday() => DateTime.Today;
}
