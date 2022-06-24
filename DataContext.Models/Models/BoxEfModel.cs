using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataContext.Models.Models
{
    [Index(nameof(Id), Name = "Id")]
    public class BoxEfModel
    {
        [Key]
        [Column(TypeName = "INTEGER")]
        public int Id { get; set; }

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

        [Column(TypeName = "INTEGER")]
        public int? PalletId { get; set; }

        [ForeignKey(nameof(PalletId))]
        [InverseProperty("Boxes")]
        public PalletEfModel? Pallet { get; set; }
    }
}
