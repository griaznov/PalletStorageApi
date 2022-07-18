using Microsoft.EntityFrameworkCore;
using DataContext.Entities;
using DataContext.EntityConfigurations;

namespace DataContext;

public class StorageContext : DbContext, IStorageContext
{
    private readonly string dataFileName;

    public virtual DbSet<Box> Boxes => Set<Box>();
    public virtual DbSet<Pallet> Pallets => Set<Pallet>();

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
        modelBuilder.ApplyConfiguration(new BoxConfiguration());
        modelBuilder.ApplyConfiguration(new PalletConfiguration());
    }

    private void CheckTablesCreated()
    {
        if ((Boxes is null) || (Pallets is null))
        {
            throw new DbUpdateException("False with reading main tables from database!");
        }
    }
}
