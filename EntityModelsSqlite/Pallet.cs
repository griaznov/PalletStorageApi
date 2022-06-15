using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EntityModelsSqlite
{
    public partial class Pallet
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
    }
}
