using Microsoft.AspNetCore.Mvc;
using PalletStorage.Common.CommonClasses;
using PalletStorage.Repositories.Repositories;
using PalletStorage.WebApi.Models.Converters;
using PalletStorage.WebApi.Models.Models;

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
    [ProducesResponseType(200, Type = typeof(IEnumerable<PalletApiModel>))]
    public async Task<IEnumerable<PalletApiModel>> GetPallets(int count = 0, int skip = 0)
    {
        var pallets = await repo.RetrieveAllAsync(count, skip);

        return pallets.Select(box => box.ToApiModel()).AsEnumerable();
    }

    // GET: api/pallets/[id]
    [HttpGet("{id:int}", Name = nameof(GetPallet))] // named route
    [ProducesResponseType(200, Type = typeof(PalletApiModel))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetPallet(int id)
    {
        Pallet? pallet = await repo.RetrieveAsync(id);

        if (pallet == null)
        {
            return NotFound(); // 404 Resource not found
        }

        return Ok(pallet.ToApiModel()); // 200 OK with customer in body
    }

    // POST: api/pallets
    // BODY: pallets (JSON, XML)
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(PalletApiModel))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] PalletApiModel pallet)
    {
        if (pallet == null)
        {
            return BadRequest(); // 400 Bad request
        }

        Pallet? addedPallet = await repo.CreateAsync(pallet.ToCommonModel());

        if (addedPallet == null)
        {
            return BadRequest("Repository failed to create new pallet.");
        }

        return CreatedAtRoute( // 201 Created
            routeName: nameof(GetPallet),
            routeValues: new { id = addedPallet.Id },
            value: addedPallet);
    }

    // PUT: api/pallets
    // BODY: Pallet (JSON, XML)
    [HttpPut("{pallet}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update([FromBody] PalletApiModel pallet)
    {
        if (pallet?.Id == null)
        {
            return BadRequest(); // 400 Bad request
        }

        var existing = await repo.UpdateAsync(pallet.ToCommonModel());

        if (existing == null)
        {
            return NotFound(); // 404 Resource not found
        }

        return new NoContentResult(); // 204 No content
    }

    // DELETE: api/pallets/[id]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await repo.DeleteAsync(id);

        if (deleted == null)
        {
            return NotFound(); // 404 Resource not found
        }

        if (deleted.Value) // short circuit AND
        {
            return new NoContentResult(); // 204 No content
        }
        else
        {
            return BadRequest( // 400 Bad request
                $"Pallet {id} was found but failed to delete.");
        }
    }

    // PUT: api/pallets/AddBoxToPallet
    // BODY: Box (JSON, XML)
    [HttpPut("AddBoxToPallet/{Box}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AddBoxToPallet([FromBody] BoxApiModel box, int palletId)
    {
        if (box?.Id == null)
        {
            return BadRequest(); // 400 Bad request
        }

        var existing = await repo.AddBoxToPalletAsync(box.ToCommonModel(), palletId);

        if (existing == null)
        {
            return NotFound(); // 404 Resource not found
        }

        return new NoContentResult(); // 204 No content
    }

    // DELETE: api/pallets/DeleteBoxFromPallet
    // BODY: Box (JSON, XML)
    [HttpDelete("DeleteBoxFromPallet/{Box}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteBoxFromPallet([FromBody] BoxApiModel box)
    {
        if (box?.Id == null)
        {
            return BadRequest(); // 400 Bad request
        }

        var existing = await repo.DeleteBoxFromPalletAsync(box.ToCommonModel());

        if (existing == null)
        {
            return NotFound(); // 404 Resource not found
        }

        return new NoContentResult(); // 204 No content
    }
}
