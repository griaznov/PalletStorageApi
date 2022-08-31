using Microsoft.EntityFrameworkCore;
using DataContext.Entities.AbstractEntities;

namespace DataContext.Entities
{
    [Index(nameof(Id), Name = "Id")]
    public class Box : IUniversalBox
    {
        public int Id { get; set; }
        public DateTime ProductionDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double Weight { get; set; }
        public int? PalletId { get; set; }
        public Pallet? Pallet { get; set; }
    }
}
