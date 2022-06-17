using System.Text;
using static System.Console;

namespace PalletStorage.Common;

public class Box : UniversalBox
{
    private const int MinDaysExpirationDate = 100;

    public Guid Id { get; }
    public DateTime ProductionDate { get; }
    public DateTime ExpirationDate { get; }

    public Box(
        double width,
        double length,
        double height,
        double weight,
        DateTime productionDate = default,
        DateTime expirationDate = default,
        Guid id = default)
        : base(width, length, height, weight)
    {
        // Verifying parameters
        // The box must have an expiration date or production date.
        if (!ValidateDateParams(productionDate, expirationDate))
        {
            StringBuilder stringBuilder = new();

            stringBuilder.AppendFormat("It is required to specify the expiration date or production date! ");
            stringBuilder.AppendFormat($"Expiration date: {expirationDate}, Production date: {productionDate}");
            WriteLine(stringBuilder.ToString());

            throw new ArgumentOutOfRangeException(stringBuilder.ToString());
        }

        // If only the production date is specified,
        // the expiration date is calculated from the production date plus 100 days
        if (expirationDate == default)
        {
            expirationDate = productionDate.AddDays(MinDaysExpirationDate);
        }

        ProductionDate = productionDate;
        ExpirationDate = expirationDate;
        Id = id;

        if (Id == default)
        { Id = Guid.NewGuid(); }
    }

    public override int GetHashCode() => Id.GetHashCode();

    public override bool Equals(object? obj) => obj is not null && Equals((Box)obj);

    public static Box Create(double width,
        double length,
        double height,
        double weight,
        DateTime prodDate = default,
        DateTime expDate = default)
    {
        return new Box(width, length, height, weight, prodDate, expDate);
    }

    public static bool ValidateDateParams(DateTime prodDate = default, DateTime expDate = default)
    {
        return (expDate != default) || (prodDate != default);
    }

    private bool Equals(Box box) => (box.Id == Id);
}
