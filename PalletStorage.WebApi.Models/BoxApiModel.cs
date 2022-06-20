namespace PalletStorage.WebApi.Models
{
    public class BoxApiModel
    {
        public Guid? Id { get; set; }
        public DateTime? ProductionDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public double? Height { get; set; }
        public double? Width { get; set; }
        public double? Length { get; set; }
        public double? Weight { get; set; }
        public double? Volume { get; set; }
        public Guid? PalletId { get; set; }
    }
}
