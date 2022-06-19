using EntityContext.Sqlite;
using PalletStorage.Common.CommonClasses;

namespace PalletStorage.EfConverters;

public static class BoxEfConverter
{
    public static Box ToCommonModel(this BoxEfModel model) =>
        new(model.Width ?? 0,
            model.Length ?? 0,
            model.Height ?? 0,
            model.Weight ?? 0,
            model.ProductionDate ?? default,
            model.ExpirationDate ?? default,
            //model.Id ?? default);
            model.Id);

    public static BoxEfModel ToEfModel(this Box input) => new()
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
