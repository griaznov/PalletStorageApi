using DataContext.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace DataContext;

public class StorageDataContext : DbContext
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

    public virtual DbSet<BoxEfModel> Boxes => Set<BoxEfModel>();
    public virtual DbSet<PalletEfModel> Pallets => Set<PalletEfModel>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite($"Filename={dataFileName}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Box
        modelBuilder.Entity<BoxEfModel>()
            .HasKey(box => box.Id);

        modelBuilder.Entity<BoxEfModel>()
            .Property(box => box.Length)
            .HasColumnType("DOUBLE")
            .IsRequired();

        modelBuilder.Entity<BoxEfModel>()
            .Property(box => box.Width)
            .HasColumnType("DOUBLE")
            .IsRequired();

        modelBuilder.Entity<BoxEfModel>()
            .Property(box => box.Height)
            .HasColumnType("DOUBLE")
            .IsRequired();

        modelBuilder.Entity<BoxEfModel>()
            .Property(box => box.Weight)
            .HasColumnType("DOUBLE")
            .IsRequired();

        modelBuilder.Entity<BoxEfModel>()
            .Property(box => box.ProductionDate)
            .HasColumnType("DATETIME")
            .IsRequired();

        modelBuilder.Entity<BoxEfModel>()
            .Property(box => box.ExpirationDate)
            .HasColumnType("DATETIME")
            .IsRequired();

        modelBuilder.Entity<BoxEfModel>()
            .Property(box => box.PalletId)
            .HasColumnType("INTEGER");

        modelBuilder.Entity<BoxEfModel>()
            .HasOne(b => b.Pallet)
            .WithMany(p => p.Boxes)
            .HasForeignKey(b => b.PalletId);

        // Pallet
        modelBuilder.Entity<PalletEfModel>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<PalletEfModel>()
            .Property(p => p.Length)
            .HasColumnType("DOUBLE")
            .IsRequired();

        modelBuilder.Entity<PalletEfModel>()
            .Property(p => p.Width)
            .HasColumnType("DOUBLE")
            .IsRequired();

        modelBuilder.Entity<PalletEfModel>()
            .Property(p => p.Height)
            .HasColumnType("DOUBLE")
            .IsRequired();

        modelBuilder.Entity<PalletEfModel>()
            .Property(p => p.PalletWeight)
            .HasColumnType("DOUBLE")
            .IsRequired();

        modelBuilder.Entity<PalletEfModel>()
            .HasMany(p => p.Boxes);
    }
}
