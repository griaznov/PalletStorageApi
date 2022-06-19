using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EntityContext.Sqlite
{
    [Index(nameof(Id), Name = "Id")]
    public partial class BoxEfModel
    {
        [Key]
        [Column(TypeName = "INTEGER")]
        public int BoxId { get; set; }

        [Required]
        [Column(TypeName = "GUID")]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "DATETIME")]
        public DateTime? ProductionDate { get; set; }

        [Required]
        [Column(TypeName = "DATETIME")]
        public DateTime? ExpirationDate { get; set; }

        [Required]
        [Column(TypeName = "DOUBLE")]
        public double? Height { get; set; }

        [Required]
        [Column(TypeName = "DOUBLE")]
        public double? Width { get; set; }

        [Required]
        [Column(TypeName = "DOUBLE")]
        public double? Length { get; set; }

        [Required]
        [Column(TypeName = "DOUBLE")]
        public double? Weight { get; set; }

        [Required]
        [Column(TypeName = "DOUBLE")]
        public double? Volume { get; set; }

        [Column(TypeName = "GUID")]
        public Guid? PalletId { get; set; }

        [ForeignKey(nameof(PalletId))]
        [InverseProperty("Boxes")]
        public virtual PalletEfModel? Pallet { get; set; }
    }
}
