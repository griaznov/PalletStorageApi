using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EntityModelsSqlite
{
    public partial class Box
    {
        [Key]
        [Column(TypeName = "INT")]
        public int BoxId { get; set; }
        [Column(TypeName = "GUID")]
        public Guid Id { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime? ProductionDate { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime? ExpirationDate { get; set; }
        [Column(TypeName = "DOUBLE")]
        public double? Height { get; set; }
        [Column(TypeName = "DOUBLE")]
        public double? Width { get; set; }
        [Column(TypeName = "DOUBLE")]
        public double? Length { get; set; }
        [Column(TypeName = "DOUBLE")]
        public double? Weight { get; set; }
        [Column(TypeName = "DOUBLE")]
        public double? Volume { get; set; }
        [Column(TypeName = "GUID")]
        public Guid? PalletId { get; set; }
    }
}
