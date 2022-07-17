using Xunit;
using AutoMapper;
using DataContext;
using DataContext.Models.MappingProfiles;
using Microsoft.EntityFrameworkCore;
using PalletStorage.WebApi.Controllers;
using PalletStorage.WebApi.Models.MappingProfiles;
using PalletStorage.WebApi.Validators.Box;
using PalletStorage.WebApi.Validators.Pallet;
using PalletStorage.Repositories;
using PalletStorage.Repositories.Boxes;
using PalletStorage.Repositories.Pallets;

namespace PalletStorage.Tests;

public class StorageContextFixture : IDisposable, IAsyncLifetime
{
    public string FilePath { get; }
    public IStorageContext Db { get; }
    public IPalletRepository PalletRepo { get; }
    public IBoxRepository BoxRepo { get; }
    public PalletController PalletController { get; }
    public BoxController BoxController { get; }

    public StorageContextFixture()
    {
        FilePath = FilesOperations.GenerateFileName("db");

        // the database will be created at the stage InitializeAsync(), after constructor.
        Db = new StorageContext(FilePath);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(MappingProfileApi));
            cfg.AddProfile(typeof(MappingProfileEntity));
        });

        var mapper = config.CreateMapper();

        PalletRepo = new PalletRepository(Db, mapper);
        BoxRepo = new BoxRepository(Db, mapper);

        PalletController = new PalletController(PalletRepo, mapper, new PalletCreateRequestValidator(), new PalletUpdateRequestValidator());
        BoxController = new BoxController(BoxRepo, mapper, new BoxCreateRequestValidator(), new BoxUpdateRequestValidator());
    }

    /// <summary>
    /// Called immediately after the class has been created, before it is used.
    /// </summary>
    public async Task InitializeAsync()
    {
        var dbIsCreated = await Db.CreateDatabaseAsync(FilePath);

        if (!dbIsCreated)
        {
            throw new DbUpdateException($"Error with creating database in {FilePath}");
        }
    }

    /// <summary>
    /// Called when an object is no longer needed
    /// </summary>
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        Db.Database.EnsureDeleted();
        Db.Dispose();
        // TODO - Ask, Need or no?
        //GC.SuppressFinalize(this);
    }

    [CollectionDefinition("StorageContextCollectionFixture")]
    public class StorageContextCollection : ICollectionFixture<StorageContextFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}


