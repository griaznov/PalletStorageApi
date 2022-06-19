using EntityContext.Sqlite;
using PalletStorage.Common.CommonClasses;

namespace PalletStorage.EfConverters;

public static class PalletEfConverter
{
    public static Pallet ToCommonModel(this PalletEfModel model) => new(
        model.Width ?? 0,
        model.Length ?? 0,
        model.Height ?? 0,
        model.PalletWeight ?? 0,
        id: model.Id ?? default,
        boxes: model.Boxes.Select(item => item.ToCommonModel()).ToList());

    public static PalletEfModel ToEfModel(this Pallet input) => new()
    {
        Width = input.Width,
        Length = input.Length,
        Height = input.Height,
        PalletWeight = input.Weight,
        //Volume = input.Volume,
        Id = input.Id,
        Boxes = input.Boxes.Select(item => item.ToEfModel()).ToList(),
    };
}
