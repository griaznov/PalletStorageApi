using EntityContext.Sqlite;
using Microsoft.AspNetCore.Mvc;
using PalletStorage.WebApi.Repositories;

namespace PalletStorage.WebApi.Controllers;

// base address: api/boxes
[Route("api/[controller]")]
[ApiController]
public class BoxesController : ControllerBase
{
    private readonly IBoxRepository repo;

    // constructor injects repository registered in Startup
    public BoxesController(IBoxRepository repo)
    {
        this.repo = repo;
    }

    // GET: api/boxes
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<BoxEfModel>))]
    public async Task<IEnumerable<BoxEfModel>> GetBoxes()
    {
        return await repo.RetrieveAllAsync();
    }
}
