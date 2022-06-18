using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.IO.Path;
using DataContext.Sqlite;
using EntityContext.Sqlite;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace PalletStorage.Tests.DataContext;

public class DataContextTests
{
    private static string GenerateFileName(string extension = "")
    {
        return string.Concat(GetRandomFileName().Replace(".", ""),
            (!string.IsNullOrEmpty(extension)) ? (extension.StartsWith(".") ? extension : string.Concat(".", extension)) : "");
    }

    private static void DeleteFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }


    [Fact(DisplayName = "1. The database can be created by models")]
    public async Task CreateDatabase()
    {
        // Assign
        var fileName = GenerateFileName("db");

        // Act
        await using StorageDataContext db = await DataContextCreator.CreateDataContextAsync(fileName);

        // Assert
        File.Exists(fileName).Should().BeTrue("Because database must be created by models");

        //DeleteFile(fileName);
    }

    [Fact(DisplayName = "2. Add a box model to the database")]
    public async void AddBox()
    {
        // Assign
        var fileName = GenerateFileName("db");

        await using StorageDataContext db = await DataContextCreator.CreateDataContextAsync(fileName);

        var box = new BoxEfModel()
        {
            Width = 1,
            Length = 1,
            Height = 1,
            Weight = 1,
            Volume = 1,
            ProductionDate = DateTime.Today,
            ExpirationDate = DateTime.Today,
            Id = new Guid()
        };

        // Act
        await db.Boxes.AddAsync(box);
        await db.SaveChangesAsync();

        // Assert
        db.Boxes.Should().HaveCount(1);
        db.Boxes.Should().Contain(box);

        //DeleteFile(fileName);
    }


    [Fact(DisplayName = "3. Add a pallet model to the database")]
    public async void AddPallet()
    {
        // Assign
        var fileName = GenerateFileName("db");

        await using StorageDataContext db = await DataContextCreator.CreateDataContextAsync(fileName);

        var pallet = new PalletEfModel()
        {
            Width = 1,
            Length = 1,
            Height = 1,
            Id = new Guid()
        };

        // Act
        await db.Pallets.AddAsync(pallet);
        await db.SaveChangesAsync();

        // Assert
        db.Pallets.Should().HaveCount(1);
        db.Pallets.Should().Contain(pallet);

        //DeleteFile(fileName);
    }

    [Fact(DisplayName = "4. Add a pallet model containing box models to the database")]
    public async void AddPalletWithBoxes()
    {
        // Assign
        var fileName = GenerateFileName("db");

        await using StorageDataContext db = await DataContextCreator.CreateDataContextAsync(fileName);

        var palletId = new Guid();

        // Pallet - owner
        var pallet = new PalletEfModel()
        {
            Width = 1,
            Length = 1,
            Height = 1,
            Id = palletId
        };

        // Box with id ref to pallet-owner
        var box = new BoxEfModel()
        {
            Width = 1,
            Length = 1,
            Height = 1,
            Weight = 1,
            Volume = 1,
            ProductionDate = DateTime.Today,
            ExpirationDate = DateTime.Today,
            Id = new Guid(),
            PalletId = palletId
        };

        // Act
        await db.Pallets.AddAsync(pallet);
        await db.Boxes.AddAsync(box);
        await db.SaveChangesAsync();

        // Assert
        db.Pallets.Should().Contain(pallet);

        db.Pallets
            .Include(p => p.Boxes)
            .First(p => p.Id == palletId)
            .Boxes.Contains(box)
            .Should().BeTrue();

        //DeleteFile(fileName);
    }

    //[Fact(DisplayName = "5. Add a box model to the database without required field - Id(guid)")]
    //public async void AddBoxWithoutRequiredId()
    //{
    //    // Assign
    //    var fileName = GenerateFileName("db");

    //    await using StorageDataContext db = await DataContextCreator.CreateDataContextAsync(fileName);

    //    var box = new BoxEfModel()
    //    {
    //        Width = 1,
    //        Length = 1,
    //        Height = 1,
    //        Weight = 1,
    //        Volume = 1,
    //        ProductionDate = DateTime.Today,
    //        ExpirationDate = DateTime.Today,
    //        //Id = new Guid()
    //    };

    //    await db.Boxes.AddAsync(box);

    //    // Act
    //    Action action = () => db.SaveChanges();

    //    // Assert
    //    action.Should().Throw<DbUpdateException>();
    //}

}

