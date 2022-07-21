namespace PalletStorage.WebApi.Models.Pallet;

public class PalletUpdateRequest
{
    public int Id { get; set; }
    public double Height { get; set; }
    public double Width { get; set; }
    public double Length { get; set; }
}
