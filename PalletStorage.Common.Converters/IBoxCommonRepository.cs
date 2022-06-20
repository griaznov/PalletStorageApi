using PalletStorage.Common.CommonClasses;

namespace PalletStorage.Common.Controllers;

public interface IBoxRepository
{
    Task<IEnumerable<Box>> RetrieveAllAsync();
}
