using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;

namespace DataContext.Entities
{
    [Index(nameof(Id), Name = "Id")]
    public class Pallet
    {
        public int Id { get; set; }
        public double PalletWeight { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }

        [XmlIgnore]
        public virtual ICollection<Box> Boxes { get; set; } = new List<Box>();
    }
}
