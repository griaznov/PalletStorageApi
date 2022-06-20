using EntityContext.Models;
using Microsoft.AspNetCore.Mvc;
using PalletStorage.WebApi.Repositories;

namespace PalletStorage.WebApi.Controllers;

// base address: api/pallets
[Route("api/[controller]")]
[ApiController]
public class PalletController : ControllerBase
{
    private readonly IPalletRepository repo;

    // constructor injects repository registered in Startup
    public PalletController(IPalletRepository repo)
    {
        this.repo = repo;
    }

    // GET: api/pallets
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<PalletEfModel>))]
    public async Task<IEnumerable<PalletEfModel>> GetPallets()
    {
        return await repo.RetrieveAllAsync();
    }

    // GET: api/pallets/[id]
    [HttpGet("{id}", Name = nameof(GetPallet))] // named route
    [ProducesResponseType(200, Type = typeof(PalletEfModel))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetPallet(string id)
    {
        PalletEfModel? pallet = await repo.RetrieveAsync(id);
        if (pallet == null)
        {
            return NotFound(); // 404 Resource not found
        }
        return Ok(pallet); // 200 OK with customer in body
    }

    // POST: api/pallets
    // BODY: pallets (JSON, XML)
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(PalletEfModel))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] PalletEfModel pallet)
    {
        if (pallet == null)
        {
            return BadRequest(); // 400 Bad request
        }

        PalletEfModel? addedPallet = await repo.CreateAsync(pallet);

        if (addedPallet == null)
        {
            return BadRequest("Repository failed to create pallet.");
        }
        else
        {
            return CreatedAtRoute( // 201 Created
                routeName: nameof(GetPallet),
                routeValues: new { id = addedPallet.Id },
                value: addedPallet);
        }
    }

    // PUT: api/pallets/[id]
    // BODY: Pallet (JSON, XML)
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(string id, [FromBody] PalletEfModel pallet)
    {
        if (pallet == null || pallet.Id != new Guid(id))
        {
            return BadRequest(); // 400 Bad request
        }

        PalletEfModel? existing = await repo.RetrieveAsync(id);
        if (existing == null)
        {
            return NotFound(); // 404 Resource not found
        }

        await repo.UpdateAsync(id, pallet);

        return new NoContentResult(); // 204 No content
    }

    // DELETE: api/pallets/[id]
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(string id)
    {
        PalletEfModel? existing = await repo.RetrieveAsync(id);

        if (existing == null)
        {
            return NotFound(); // 404 Resource not found
        }

        var deleted = await repo.DeleteAsync(id);

        if (deleted.HasValue && deleted.Value) // short circuit AND
        {
            return new NoContentResult(); // 204 No content
        }
        else
        {
            return BadRequest( // 400 Bad request
                $"Customer {id} was found but failed to delete.");
        }
    }

}
