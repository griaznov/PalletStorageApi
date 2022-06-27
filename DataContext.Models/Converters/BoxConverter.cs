using DataContext.Models.Models;
using PalletStorage.Common.CommonClasses;

namespace DataContext.Models.Converters;

public static class BoxConverter
{
    public static Box ToCommonModel(this BoxEfModel model) =>
        new(model.Width ?? 0,
            model.Length ?? 0,
            model.Height ?? 0,
            model.Weight ?? 0,
            model.ProductionDate ?? default,
            model.ExpirationDate ?? default,
            model.Id
            );

    public static BoxEfModel ToEfModel(this Box input) => new()
    {
        Width = input.Width,
        Length = input.Length,
        Height = input.Height,
        Weight = input.Weight,
        ProductionDate = input.ProductionDate,
        ExpirationDate = input.ExpirationDate,
        Id = input.Id
    };
}
