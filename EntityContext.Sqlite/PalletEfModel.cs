using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;

namespace EntityContext.Sqlite
{
    [Index(nameof(Id), Name = "Id")]
    public partial class PalletEfModel
    {
        [Key]
        [Column(TypeName = "INTEGER")]
        public int PalletId { get; set; }

        [Required]
        [Column(TypeName = "GUID")]
        public Guid? Id { get; set; }

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
        public virtual ICollection<BoxEfModel> Boxes { get; set; } = new HashSet<BoxEfModel>();
    }
}
