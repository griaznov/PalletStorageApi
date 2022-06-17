using EntityModels;
using PalletStorage.Common;

namespace PalletStorage.Converters;

public static class BoxConverter
{
    public static Box FromModel(this BoxModel model) =>
        new(model.Width ?? 0,
            model.Length ?? 0,
            model.Height ?? 0,
            model.Weight ?? 0,
            model.ProductionDate ?? default,
            model.ExpirationDate ?? default,
            model.Id);

    public static BoxModel ToModel(this Box input) => new()
    {
        Width = input.Width,
        Length = input.Length,
        Height = input.Height,
        Weight = input.Weight,
        Volume = input.Volume,
        ProductionDate = input.ProductionDate,
        ExpirationDate = input.ExpirationDate,
        Id = input.Id
    };
}
