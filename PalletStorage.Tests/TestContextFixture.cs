using Xunit;
using AutoMapper;
using DataContext;
using DataContext.Models.MappingProfiles;
using PalletStorage.Repositories.Repositories;
using PalletStorage.WebApi.Controllers;
using PalletStorage.WebApi.Models.MappingProfiles;
using PalletStorage.WebApi.Validators;

namespace PalletStorage.Tests;

public class StorageContextFixture : IDisposable
{
    public readonly string FilePath;
    public readonly IStorageContext Db;

    public readonly IPalletRepository PalletRepo;
    public readonly IBoxRepository BoxRepo;

    public readonly PalletController PalletController;
    public readonly BoxController BoxController;

    public StorageContextFixture()
    {
        FilePath = FilesOperations.GenerateFileName("db");
        Db = StorageContext.CreateContextAsync(FilePath).GetAwaiter().GetResult();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(MappingProfileApi));
            cfg.AddProfile(typeof(MappingProfileEntity));
        });

        var mapper = config.CreateMapper();
        
        PalletRepo = new PalletRepository(Db, mapper);
        BoxRepo = new BoxRepository(Db, mapper);

        PalletController = new PalletController(PalletRepo, mapper, new PalletValidator());
        BoxController = new BoxController(BoxRepo, mapper, new BoxValidator());
    }

    public void Dispose()
    {
        Db.Database.EnsureDeleted();
        Db.Dispose();
    }

    [CollectionDefinition("StorageContextCollectionFixture")]
    public class StorageContextCollection : ICollectionFixture<StorageContextFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}


