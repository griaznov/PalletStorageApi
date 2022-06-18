using EntityModels.Sqlite;
using PalletStorage.Common;

namespace PalletStorage.Converters;

public static class PalletConverter
{
    public static Pallet FromModel(this PalletModel model) => new(
        model.Width ?? 0,
        model.Length ?? 0,
        model.Height ?? 0,
        model.Weight ?? 0,
        id: model.Id,
        boxes: model.Boxes.Select(item => item.FromModel()).ToList());

    public static PalletModel ToModel(this Pallet input) => new()
    {
        Width = input.Width,
        Length = input.Length,
        Height = input.Height,
        Weight = input.Weight,
        //Volume = input.Volume,
        Id = input.Id,
        Boxes = input.Boxes.Select(item => item.ToModel()).ToList(),
    };
}
