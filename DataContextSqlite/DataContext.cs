using EntityModels;
using EntityModelsSqlite;
using Microsoft.EntityFrameworkCore;

namespace DataContextSqlite
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public virtual DbSet<BoxModel> Boxes { get; set; } = null!;
        public virtual DbSet<PalletModel> Pallets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Filename=../PalletStorage.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BoxModel>(entity =>
            {
                entity.Property(e => e.Height).HasDefaultValueSql("0");
                entity.Property(e => e.Length).HasDefaultValueSql("0");
                entity.Property(e => e.Volume).HasDefaultValueSql("0");
                entity.Property(e => e.Weight).HasDefaultValueSql("0");
                entity.Property(e => e.Width).HasDefaultValueSql("0");
            });

            modelBuilder.Entity<BoxModel>()
                .HasOne(p => p.Pallet)
                .WithMany(t => t.Boxes)
                .HasForeignKey(p => p.PalletId)
                .HasPrincipalKey(t => t.Id);

            modelBuilder.Entity<PalletModel>(entity =>
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
