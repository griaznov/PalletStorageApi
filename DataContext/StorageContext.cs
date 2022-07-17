using DataContext.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataContext;

public class StorageContext : DbContext, IStorageContext
{
    private readonly string dataFileName;

    public StorageContext(string dataFileName = "../PalletStorage.db")
    {
        this.dataFileName = dataFileName;
        CheckTablesCreated();
    }

    public StorageContext(DbContextOptions<StorageContext> options, string dataFileName = "../PalletStorage.db") : base(options)
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

    public virtual DbSet<Box> Boxes => Set<Box>();
    public virtual DbSet<Pallet> Pallets => Set<Pallet>();

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

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
        modelBuilder.Entity<Box>()
            .HasKey(box => box.Id);

        modelBuilder.Entity<Box>()
            .Property(box => box.Length)
            .HasColumnType("DOUBLE")
            .IsRequired();

        modelBuilder.Entity<Box>()
            .Property(box => box.Width)
            .HasColumnType("DOUBLE")
            .IsRequired();

        modelBuilder.Entity<Box>()
            .Property(box => box.Height)
            .HasColumnType("DOUBLE")
            .IsRequired();

        modelBuilder.Entity<Box>()
            .Property(box => box.Weight)
            .HasColumnType("DOUBLE")
            .IsRequired();

        modelBuilder.Entity<Box>()
            .Property(box => box.ProductionDate)
            .HasColumnType("DATETIME")
            .IsRequired();

        modelBuilder.Entity<Box>()
            .Property(box => box.ExpirationDate)
            .HasColumnType("DATETIME")
            .IsRequired();

        modelBuilder.Entity<Box>()
            .Property(box => box.PalletId)
            .HasColumnType("INTEGER");

        modelBuilder.Entity<Box>()
            .HasOne(b => b.Pallet)
            .WithMany(p => p.Boxes)
            .HasForeignKey(b => b.PalletId);

        // Pallet
        modelBuilder.Entity<Pallet>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Pallet>()
            .Property(p => p.Length)
            .HasColumnType("DOUBLE")
            .IsRequired();

        modelBuilder.Entity<Pallet>()
            .Property(p => p.Width)
            .HasColumnType("DOUBLE")
            .IsRequired();

        modelBuilder.Entity<Pallet>()
            .Property(p => p.Height)
            .HasColumnType("DOUBLE")
            .IsRequired();

        modelBuilder.Entity<Pallet>()
            .Property(p => p.PalletWeight)
            .HasColumnType("DOUBLE")
            .IsRequired();

        modelBuilder.Entity<Pallet>()
            .HasMany(p => p.Boxes);
    }
}
