//using PalletStorage.Common.CommonClasses;
//using PalletStorage.WebApi.Models.Models;

//namespace PalletStorage.WebApi.Models.Converters
//{
//    public static class BoxConverter
//    {
//        public static Box ToCommonModel(this BoxApiModel model) =>
//            new(model.Width,
//                model.Length,
//                model.Height,
//                model.Weight,
//                model.ProductionDate,
//                model.ExpirationDate,
//                model.Id ?? default);

//        public static BoxApiModel ToApiModel(this Box input) => new()
//        {
//            Width = input.Width,
//            Length = input.Length,
//            Height = input.Height,
//            Weight = input.Weight,
//            ProductionDate = input.ProductionDate,
//            ExpirationDate = input.ExpirationDate,
//            Id = input.Id
//        };
//    }
//}
