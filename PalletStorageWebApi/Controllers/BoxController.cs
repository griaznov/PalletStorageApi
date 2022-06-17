using EntityModels;
using EntityModelsSqlite;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PalletStorageWebApi.Repositories;

namespace PalletStorageWebApi.Controllers;

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
    [ProducesResponseType(200, Type = typeof(IEnumerable<BoxModel>))]
    public async Task<IEnumerable<BoxModel>> GetBoxes()
    {
        return await repo.RetrieveAllAsync();
    }
}
