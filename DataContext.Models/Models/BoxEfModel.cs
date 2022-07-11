using Microsoft.EntityFrameworkCore;

namespace DataContext.Models.Models
{
    [Index(nameof(Id), Name = "Id")]
    public class BoxEfModel
    {
        public int Id { get; set; }
        public DateTime ProductionDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double Weight { get; set; }
        public int? PalletId { get; set; }
        public PalletEfModel? Pallet { get; set; }
    }
}
