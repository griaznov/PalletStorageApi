using PalletStorage.Common.CommonClasses;
using PalletStorage.WebApi.Models.Models;

namespace PalletStorage.WebApi.Models.Converters
{
    public static class BoxConverter
    {
        public static Box ToCommonModel(this BoxApiModel model) =>
            new(model.Width ?? 0,
                model.Length ?? 0,
                model.Height ?? 0,
                model.Weight ?? 0,
                model.ProductionDate ?? default,
                model.ExpirationDate ?? default,
                model.Id ?? default);
                //model.Id);

        public static BoxApiModel ToApiModel(this Box input) => new()
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
}
