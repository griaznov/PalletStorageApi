using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;

namespace DataContext.Models.Models
{
    [Index(nameof(Id), Name = "Id")]
    public class PalletEfModel
    {
        public int Id { get; set; }
        public double PalletWeight { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }

        [XmlIgnore]
        public virtual ICollection<BoxEfModel> Boxes { get; set; } = new List<BoxEfModel>();
    }
}
