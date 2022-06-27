using DataContext.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace DataContext
{
    public partial class StorageDataContext : DbContext
    {
        private readonly string dataFileName;

        public StorageDataContext(string dataFileName = "../PalletStorage.db")
        {
            this.dataFileName = dataFileName;
            CheckTablesCreated();
        }

        public StorageDataContext(DbContextOptions<StorageDataContext> options, string dataFileName = "../PalletStorage.db") : base(options)
        {
            this.dataFileName = dataFileName;
            CheckTablesCreated();
        }

        private void CheckTablesCreated()
        {
            if ((Boxes is null) || (Pallets is null))
            {
                throw new DbUpdateException("False with reading main tables from database!");
            }
        }

        public virtual DbSet<BoxEfModel> Boxes { get; set; } = null!;

        public virtual DbSet<PalletEfModel> Pallets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Filename={dataFileName}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BoxEfModel>(entity =>
            {
                entity.Property(e => e.Height).HasDefaultValueSql("0");
                entity.Property(e => e.Length).HasDefaultValueSql("0");
                entity.Property(e => e.Weight).HasDefaultValueSql("0");
                entity.Property(e => e.Width).HasDefaultValueSql("0");
            });

            modelBuilder.Entity<BoxEfModel>()
                .HasOne(p => p.Pallet)
                .WithMany(t => t.Boxes)
                .HasForeignKey(p => p.PalletId);

            modelBuilder.Entity<PalletEfModel>(entity =>
            {
                entity.Property(e => e.Height).HasDefaultValueSql("0");
                entity.Property(e => e.Length).HasDefaultValueSql("0");
                entity.Property(e => e.PalletWeight).HasDefaultValueSql("0");
                entity.Property(e => e.Width).HasDefaultValueSql("0");
            });

            modelBuilder.Entity<PalletEfModel>()
                .HasMany(p => p.Boxes);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
