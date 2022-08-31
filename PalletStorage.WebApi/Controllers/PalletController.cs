using System.ComponentModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PalletStorage.BusinessModels;
using PalletStorage.Repositories.Pallets;
using PalletStorage.WebApi.Models.Additional;
using PalletStorage.WebApi.Models.Box;
using PalletStorage.WebApi.Models.Pallet;

namespace PalletStorage.WebApi.Controllers;

// base address: api/pallets
[Route("api/[controller]")]
[ApiController]
public class PalletController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IPalletRepository repo;

    // constructor injects repository registered in Startup/Program
    public PalletController(IPalletRepository repo, IMapper mapper)
    {
        this.repo = repo;
        this.mapper = mapper;
    }

    // GET: api/pallets
    [HttpGet(Name = "GetPallets")]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<PalletResponse>))]
    public async Task<IReadOnlyList<PalletResponse>> GetPallets(
        [FromQuery] PaginationFilter filter,
        CancellationToken token)
    {
        var pallets = await repo.GetAllAsync(filter.Take, filter.Skip, token);

        return mapper.Map<IReadOnlyList<PalletResponse>>(pallets);
    }

    // GET: api/pallets/[id]
    [HttpGet("{id:int}", Name = nameof(GetPallet))] // named route
    [ProducesResponseType(200, Type = typeof(PalletResponse))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetPallet(int id, CancellationToken token)
    {
        PalletModel? pallet = await repo.GetAsync(id, token);
        
        if (pallet == null)
        {
            return NotFound(); // 404 Resource not found
        }

        return Ok(mapper.Map<PalletResponse>(pallet)); // 200 OK with customer in body
    }

    // POST: api/pallets
    // BODY: pallets (JSON, XML)
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(PalletResponse))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] PalletCreateRequest pallet, CancellationToken token)
    {
        var addedPallet = await repo.CreateAsync(mapper.Map<PalletModel>(pallet), token);

        if (addedPallet == null)
        {
            return BadRequest("Repository failed to create new pallet.");
        }

        return Created($"{Request.Path}/{addedPallet.Id}", mapper.Map<PalletResponse>(addedPallet));
    }

    // PUT: api/pallets
    // BODY: Pallet (JSON, XML)
    [HttpPut("{pallet}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update([FromBody] PalletUpdateRequest pallet, CancellationToken token)
    {
        var existing = await repo.UpdateAsync(mapper.Map<PalletModel>(pallet), token);

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
    public async Task<IActionResult> Delete(int id, CancellationToken token)
    {
        var deleted = await repo.DeleteAsync(id, token);

        if (deleted) // short circuit AND
        {
            return new NoContentResult(); // 204 No content
        }
        else
        {
            return BadRequest( // 400 Bad request
                $"Pallet {id} was not found or failed to delete.");
        }
    }

    // PUT: api/pallets/AddBoxToPallet
    // BODY: Box (JSON, XML)
    [HttpPut("AddBoxToPallet/{Box}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AddBoxToPallet([FromBody] BoxUpdateRequest box, int palletId, CancellationToken token)
    {
        var existing = await repo.AddBoxToPalletAsync(mapper.Map<BoxModel>(box), palletId, token);

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
    public async Task<IActionResult> DeleteBoxFromPallet([FromBody] BoxUpdateRequest box, CancellationToken token)
    {
        var existing = await repo.DeleteBoxFromPalletAsync(mapper.Map<BoxModel>(box), token);

        if (existing == null)
        {
            return NotFound(); // 404 Resource not found
        }

        return new NoContentResult(); // 204 No content
    }
}
