namespace PalletStorage.WebApi.Models.Models
{
    public class PalletApiModel
    {
        public int? Id { get; set; }
        public double? PalletWeight { get; set; }
        public double? Height { get; set; }
        public double? Width { get; set; }
        public double? Length { get; set; }

        //public virtual List<BoxApiModel> Boxes { get; set; } = new();
        public virtual List<BoxApiModel>? Boxes { get; set; }
    }
}
