namespace PalletStorage.Business.Models;

public class PalletModel : UniversalBoxModel
{
    private const double DefaultPalletWeight = 30;

    public double PalletWeight { get; }
    public int Id { get; }
    public List<BoxModel> Boxes { get; }
    public override double Weight => PalletWeight + Boxes.Sum(b => b.Weight);

    public override double Volume => base.Volume + Boxes.Sum(b => b.Volume);
    public DateTime ExpirationDate => Boxes.Count == 0 ? default : Boxes.Min(box => box.ExpirationDate);

    public PalletModel(double width,
        double length,
        double height,
        int id = default,
        List<BoxModel>? boxes = null)
        : base(width, length, height)
    {
        // default weight value for the pallet
        PalletWeight = DefaultPalletWeight;
        Boxes = boxes ?? new List<BoxModel>();
        Id = id;
    }

    public void AddBox(BoxModel box)
    {
        Boxes.Add(box);
    }

    public static PalletModel Create(double width, double length, double height)
    {
        return new PalletModel(width, length, height);
    }

    public override int GetHashCode() => Id;

    public override bool Equals(object? obj) => obj is PalletModel pallet && pallet.Id == Id;
}
