using System.Text;
using static System.Console;

namespace PalletStorage.Common;

public class Pallet : UniversalBox
{
    private const double DefaultPalletWeight = 30;

    public double PalletWeight { get; }
    public Guid Id { get; }
    public List<Box> Boxes { get; }
    public override double Weight => (PalletWeight + Boxes.Sum(b => b.Weight));
    public override double Volume => (base.Volume + Boxes.Sum(b => b.Volume));
    public DateTime ExpirationDate => Boxes.Count == 0 ? default : Boxes.Min(box => box.ExpirationDate);

    public Pallet(double width,
        double length,
        double height,
        double weight = 0,
        Guid id = default,
        List<Box>? boxes = null)
        : base(width, length, height, weight)
    {
        // default weight value for the pallet
        PalletWeight = DefaultPalletWeight;
        Boxes = boxes ?? new List<Box>();
        Id = id;

        if (Id == default)
        { Id = Guid.NewGuid(); }
    }

    public void AddBox(Box box)
    {
        Boxes.Add(box);
    }

    public static Pallet Create(double width, double length, double height)
    {
        return new Pallet(width, length, height, DefaultPalletWeight);
    }
}
