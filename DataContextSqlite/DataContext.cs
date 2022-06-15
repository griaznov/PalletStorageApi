using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityModelsSqlite
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Box> Boxes { get; set; } = null!;
        public virtual DbSet<Pallet> Pallets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlite("Filename=../PalletStorage.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Box>(entity =>
            {
                entity.Property(e => e.BoxId).ValueGeneratedNever();

                entity.Property(e => e.Height).HasDefaultValueSql("0");

                entity.Property(e => e.Length).HasDefaultValueSql("0");

                entity.Property(e => e.Volume).HasDefaultValueSql("0");

                entity.Property(e => e.Weight).HasDefaultValueSql("0");

                entity.Property(e => e.Width).HasDefaultValueSql("0");
            });

            modelBuilder.Entity<Pallet>(entity =>
            {
                entity.Property(e => e.PalletId).ValueGeneratedNever();

                entity.Property(e => e.Height).HasDefaultValueSql("0");

                entity.Property(e => e.Length).HasDefaultValueSql("0");

                entity.Property(e => e.Weight).HasDefaultValueSql("0");

                entity.Property(e => e.Width).HasDefaultValueSql("0");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
