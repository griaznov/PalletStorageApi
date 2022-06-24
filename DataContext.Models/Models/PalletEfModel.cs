using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;

namespace DataContext.Models.Models
{
    [Index(nameof(Id), Name = "Id")]
    public class PalletEfModel
    {
        [Key]
        [Column(TypeName = "INTEGER")]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "DOUBLE")]
        public double? PalletWeight { get; set; }

        [Required]
        [Column(TypeName = "DOUBLE")]
        public double? Height { get; set; }

        [Required]
        [Column("Width ", TypeName = "DOUBLE")]
        public double? Width { get; set; }

        [Required]
        [Column(TypeName = "DOUBLE")]
        public double? Length { get; set; }

        [InverseProperty(nameof(BoxEfModel.Pallet))]
        [XmlIgnore]
        public virtual ICollection<BoxEfModel> Boxes { get; set; } = new List<BoxEfModel>();
    }
}
