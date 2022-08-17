using Xunit;
using AutoMapper;
using DataContext;
using Microsoft.EntityFrameworkCore;
using PalletStorage.WebApi.Controllers;
using PalletStorage.Repositories.Boxes;
using PalletStorage.Repositories.Pallets;
using DataContext.Migrations;
using PalletStorage.Repositories.Automapper;
using PalletStorage.WebApi.Automapper;

namespace PalletStorage.Tests;

public class StorageContextFixture : IAsyncLifetime
{
    private const string ErrorMessage = "Property not initialized!";
    private IStorageContext? dbContext;
    private IPalletRepository? palletRepo;
    private IBoxRepository? boxRepo;
    private PalletController? palletController;
    private BoxController? boxController;
    private IMapper? mapper;

    public string FilePath { get; }
    public IStorageContext DbContext => dbContext ?? throw new NullReferenceException(ErrorMessage);
    public IPalletRepository PalletRepo => palletRepo ?? throw new NullReferenceException(ErrorMessage);
    public IBoxRepository BoxRepo => boxRepo ?? throw new NullReferenceException(ErrorMessage);
    public PalletController PalletController => palletController ?? throw new NullReferenceException(ErrorMessage);
    public BoxController BoxController => boxController ?? throw new NullReferenceException(ErrorMessage);
    public IMapper Mapper => mapper ?? throw new NullReferenceException(ErrorMessage);

    /// <summary>
    /// Main fixture context will be created at the stage InitializeAsync(), after constructor.
    /// </summary>
    public StorageContextFixture()
    {
        FilePath = FilesOperations.GenerateFileName("db");
    }

    /// <summary>
    /// Called immediately after the class has been created, before it is used.
    /// </summary>
    public async Task InitializeAsync()
    {
        var contextFactory = new StorageContextFactory();
        dbContext = contextFactory.CreateStorageContext(FilePath);

        await dbContext.Database.MigrateAsync().ConfigureAwait(false);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(BoxApiMappingProfile));
            cfg.AddProfile(typeof(PalletApiMappingProfile));
            cfg.AddProfile(typeof(BoxModelMappingProfile));
            cfg.AddProfile(typeof(PalletModelMappingProfile));
        });

        mapper = config.CreateMapper();

        palletRepo = new PalletRepository(dbContext, mapper);
        boxRepo = new BoxRepository(dbContext, mapper);

        palletController = new PalletController(palletRepo, mapper);
        boxController = new BoxController(BoxRepo, mapper);
    }

    /// <summary>
    /// Called when an object is no longer needed
    /// </summary>
    public Task DisposeAsync()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();

        return Task.CompletedTask;
    }

    [CollectionDefinition("StorageContextCollectionFixture")]
    public class StorageContextCollection : ICollectionFixture<StorageContextFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}


