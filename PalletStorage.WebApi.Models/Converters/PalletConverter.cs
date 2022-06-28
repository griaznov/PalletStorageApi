//using PalletStorage.Common.CommonClasses;
//using PalletStorage.WebApi.Models.Models;

//namespace PalletStorage.WebApi.Models.Converters
//{
//    public static class PalletConverter
//    {
//        public static Pallet ToCommonModel(this PalletApiModel model) => new(
//            model.Width,
//            model.Length,
//            model.Height,
//            model.PalletWeight,
//            id: model.Id ?? default,
//            boxes: model.Boxes?.Select(item => item.ToCommonModel()).ToList());

//        public static PalletApiModel ToApiModel(this Pallet input) => new()
//        {
//            Width = input.Width,
//            Length = input.Length,
//            Height = input.Height,
//            PalletWeight = input.Weight,
//            Id = input.Id,
//            Boxes = input.Boxes.Select(item => item.ToApiModel()).ToList(),
//        };
//    }
//}
