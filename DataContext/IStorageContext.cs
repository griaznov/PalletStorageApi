using DataContext.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DataContext;

public interface IStorageContext : IDisposable, IAsyncDisposable
{
    DbSet<Box> Boxes { get; }
    DbSet<Pallet> Pallets { get; }
    Task<int> SaveChangesAsync();
    DatabaseFacade Database { get; }
    static Task<IStorageContext> CreateContextAsync(string dataPath)
    {
        throw new NotImplementedException();
    }
}
