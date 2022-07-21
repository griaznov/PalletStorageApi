using System.Xml.Serialization;
using PalletStorage.WebApi.Models.Box;

namespace PalletStorage.WebApi.Models.Pallet;

public class PalletResponse
{
    public int Id { get; set; }
    public double PalletWeight { get; set; }
    public double Height { get; set; }
    public double Width { get; set; }
    public double Length { get; set; }

    [XmlIgnore]
    public virtual IReadOnlyList<BoxResponse> Boxes { get; set; } = new List<BoxResponse>();
}
