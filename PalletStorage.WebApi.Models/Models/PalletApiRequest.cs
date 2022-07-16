namespace PalletStorage.WebApi.Models.Models
{
    public class PalletApiRequest
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }

        public virtual List<BoxApiRequest>? Boxes { get; set; }
    }
}
