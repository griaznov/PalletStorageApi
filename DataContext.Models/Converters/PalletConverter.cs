using DataContext.Models.Models;
using PalletStorage.Common.CommonClasses;

namespace DataContext.Models.Converters;

public static class PalletConverter
{
    public static Pallet ToCommonModel(this PalletEfModel model) => new(
        model.Width ?? 0,
        model.Length ?? 0,
        model.Height ?? 0,
        model.PalletWeight ?? 0,
        boxes: model.Boxes.Select(item => item.ToCommonModel()).ToList(),
        id: model.Id
        );

    public static PalletEfModel ToEfModel(this Pallet input) => new()
    {
        Width = input.Width,
        Length = input.Length,
        Height = input.Height,
        PalletWeight = input.Weight,
        Boxes = input.Boxes.Select(item => item.ToEfModel()).ToList(),
        Id = input.Id
    };
}
