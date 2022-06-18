using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityModels.Sqlite
{
    //[Index(nameof(Guid), Name = "Guid")]
    public partial class PalletModel
    {
        [Key]
        [Column(TypeName = "INT")]
        public int PalletId { get; set; }

        [Column(TypeName = "GUID")]
        public Guid Id { get; set; }

        [Column("ExpirationDate ", TypeName = "DATETIME")]
        public DateTime? ExpirationDate { get; set; }

        [Column(TypeName = "DOUBLE")]
        public double? Weight { get; set; }

        [Column(TypeName = "DOUBLE")]
        public double? Height { get; set; }

        [Column("Width ", TypeName = "DOUBLE")]
        public double? Width { get; set; }

        [Column(TypeName = "DOUBLE")]
        public double? Length { get; set; }

        [InverseProperty(nameof(BoxModel.Pallet))]
        //[XmlIgnore]
        //public virtual ICollection<Box> Boxes { get; set; } = new List<Box>();
        public virtual List<BoxModel> Boxes { get; set; } = new List<BoxModel>();
    }
}
