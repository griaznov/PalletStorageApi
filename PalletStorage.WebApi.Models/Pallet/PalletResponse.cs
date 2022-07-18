using PalletStorage.WebApi.Models.Models.Box;

namespace PalletStorage.WebApi.Models.Models.Pallet
{
    public class PalletResponse
    {
        public int Id { get; set; }
        public double PalletWeight { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }

        public virtual List<BoxResponse>? Boxes { get; set; }
    }
}
