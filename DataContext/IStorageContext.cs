using DataContext.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DataContext;

public interface IStorageContext : IDisposable, IAsyncDisposable
{
    DbSet<BoxEfModel> Boxes { get; }
    DbSet<PalletEfModel> Pallets { get; }
    Task<int> SaveChangesAsync();
    DatabaseFacade Database { get; }
    static Task<IStorageContext> CreateContextAsync(string dataPath)
    {
        throw new NotImplementedException();
    }
}
