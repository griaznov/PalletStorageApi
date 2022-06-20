namespace PalletStorage.WebApi.Models
{
    public class PalletApiModel
    {
        public Guid? Id { get; set; }
        public double? PalletWeight { get; set; }
        public double? Height { get; set; }
        public double? Width { get; set; }
        public double? Length { get; set; }

        public virtual ICollection<BoxApiModel> Boxes { get; set; } = new HashSet<BoxApiModel>();
    }
}
