using Microsoft.EntityFrameworkCore;
using DataContext.Entities;

namespace DataContext;

internal sealed class StorageContext : DbContext, IStorageContext
{
    private const string DefaultDataFileName = "../PalletStorage.db";

    private readonly string dataFileName;

    public DbSet<Box> Boxes => Set<Box>();
    public DbSet<Pallet> Pallets => Set<Pallet>();

    /// <summary>
    /// Must be public for dependency injection
    /// </summary>
    public StorageContext(DbContextOptions<StorageContext> options, string dataFileName = DefaultDataFileName) : base(options)
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
            optionsBuilder
                .UseSqlite($"Data Source={dataFileName}")
                .UseLoggerFactory(new ConsoleLoggerFactory());
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
