using Microsoft.EntityFrameworkCore;
using DataContext.Entities;

namespace DataContext;

internal sealed class StorageContext : DbContext, IStorageContext
{
    private readonly string dataFileName;

    public DbSet<Box> Boxes => Set<Box>();
    public DbSet<Pallet> Pallets => Set<Pallet>();

    internal StorageContext(string dataFileName = "../PalletStorage.db")
    {
        this.dataFileName = dataFileName;
    }

    /// <summary>
    /// Must be public for dependency injection
    /// </summary>
    public StorageContext(DbContextOptions<StorageContext> options, string dataFileName = "../PalletStorage.db") : base(options)
    {
        this.dataFileName = dataFileName;
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
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
