using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using PalletStorage.BusinessModels;
using PalletStorage.Repositories.Boxes;
using PalletStorage.WebApi.Models.Box;

namespace PalletStorage.WebApi.Controllers;

// base address: api/boxes
[Route("api/[controller]")]
[ApiController]
public class BoxController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IBoxRepository repo;

    // constructor injects repository registered in Startup/Program
    public BoxController(IBoxRepository repo, IMapper mapper)
    {
        this.repo = repo;
        this.mapper = mapper;
    }

    // GET: api/boxes
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<BoxResponse>))]
    public async Task<IReadOnlyList<BoxResponse>> GetBoxes(
        [DefaultValue(100)] int take,
        [DefaultValue(0)] int skip,
        CancellationToken token)
    {
        var boxes = await repo.GetAllAsync(take, skip, token);

        return mapper.Map<IReadOnlyList<BoxResponse>>(boxes);
    }

    // GET: api/boxes/[id]
    [HttpGet("{id:int}", Name = nameof(GetBox))] // named route
    [ProducesResponseType(200, Type = typeof(BoxResponse))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetBox(int id, CancellationToken token)
    {
        BoxModel? box = await repo.GetAsync(id, token);

        if (box == null)
        {
            return NotFound(); // 404 Resource not found
        }

        return Ok(mapper.Map<BoxResponse>(box)); // 200 OK with customer in body
    }

    // POST: api/boxes
    // BODY: Box (JSON, XML)
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(BoxResponse))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] BoxCreateRequest box, CancellationToken token)
    {
        var addedBox = await repo.CreateAsync(mapper.Map<BoxModel>(box), token);

        if (addedBox == null)
        {
            return BadRequest("Repository failed to create box.");
        }

        return Created($"{Request.Path}/{addedBox.Id}", mapper.Map<BoxResponse>(addedBox));
    }

    // PUT: api/boxes
    // BODY: Box (JSON, XML)
    [HttpPut("{box}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update([FromBody] BoxUpdateRequest box, CancellationToken token)
    {
        var existing = await repo.UpdateAsync(mapper.Map<BoxModel>(box), token);

        if (existing == null)
        {
            return NotFound(); // 404 Resource not found
        }

        return new NoContentResult(); // 204 No content
    }

    // DELETE: api/boxes/[id]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Delete(int id, CancellationToken token)
    {
        var deleted = await repo.DeleteAsync(id, token);

        if (deleted)
        {
            return new NoContentResult(); // 204 No content
        }
        else
        {
            return BadRequest( // 400 Bad request
                $"Box {id} was not found or failed to delete.");
        }
    }
}
