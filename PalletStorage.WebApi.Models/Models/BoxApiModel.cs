namespace PalletStorage.WebApi.Models.Models
{
    public class BoxApiModel
    {
        public int? Id { get; set; }
        public DateTime ProductionDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double Weight { get; set; }
    }
}
