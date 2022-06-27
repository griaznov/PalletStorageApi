using DataContext.Models.Converters;
using DataContext.Models.Models;
using FluentAssertions;
using PalletStorage.Common.CommonClasses;
using PalletStorage.WebApi.Models.Converters;
using PalletStorage.WebApi.Models.Models;
using Xunit;

namespace PalletStorage.Tests.ModelEfConverters;

public class PalletApiConvertersTests
{
    [Fact(DisplayName = "1. Pallet with Boxes convert to PalletEfModel with BoxesEfModel")]
    public void PalletConvertToBoxEfModel()
    {
        // Arrange
        var pallet = Pallet.Create(2, 3, 4);
        var box = Box.Create(2, 2, 2, 2, DateTime.Today);

        pallet.AddBox(box);

        // Act
        var palletModel = pallet.ToEfModel();

        // Assert
        palletModel.Boxes.Should().HaveCount(1);
    }

    [Fact(DisplayName = "2. PalletEfModel with BoxesEfModel convert to Pallet with Boxes")]
    public void BoxEfModelConvertToBox()
    {
        // Arrange
        var boxModel = new BoxEfModel()
        {
            Id = 33,
            Width = 1,
            Length = 1,
            Height = 1,
            Weight = 1,
            ProductionDate = DateTime.Today,
            ExpirationDate = DateTime.Today,
        };

        var palletModel = new PalletEfModel()
        {
            Id = 36,
            Width = 1,
            Length = 1,
            Height = 1,
            PalletWeight = 10,
            Boxes = new List<BoxEfModel> { boxModel }
        };

        // Act
       var pallet = palletModel.ToCommonModel();

        // Assert
        pallet.Boxes.Should().HaveCount(1);
    }
}
