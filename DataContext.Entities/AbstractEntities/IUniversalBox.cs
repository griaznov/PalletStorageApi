
namespace DataContext.Entities.AbstractEntities;

public interface IUniversalBox
{
    public int Id { get; set; }

    public double Height { get; set; }

    public double Width { get; set; }

    public double Length { get; set; }
}
