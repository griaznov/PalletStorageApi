using EntityModelsSqlite;
using Microsoft.EntityFrameworkCore;

namespace DataContextSqlite
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
                optionsBuilder.UseSqlite("Filename=../PalletStorage.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Box>(entity =>
            {
                entity.Property(e => e.Height).HasDefaultValueSql("0");
                entity.Property(e => e.Length).HasDefaultValueSql("0");
                entity.Property(e => e.Volume).HasDefaultValueSql("0");
                entity.Property(e => e.Weight).HasDefaultValueSql("0");
                entity.Property(e => e.Width).HasDefaultValueSql("0");
            });

            modelBuilder.Entity<Pallet>(entity =>
            {
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
