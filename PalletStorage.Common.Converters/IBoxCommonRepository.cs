using PalletStorage.Common.CommonClasses;

namespace PalletStorage.Common.Controllers;

public interface IBoxCommonRepository
{
    Task<IEnumerable<Box>> RetrieveAllAsync();
}
