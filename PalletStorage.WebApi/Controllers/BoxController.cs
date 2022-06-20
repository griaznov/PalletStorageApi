using EntityContext.Models;
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

    // GET: api/boxes/[id]
    [HttpGet("{id}", Name = nameof(GetBox))] // named route
    [ProducesResponseType(200, Type = typeof(BoxEfModel))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetBox(string id)
    {
        BoxEfModel? box = await repo.RetrieveAsync(id);
        if (box == null)
        {
            return NotFound(); // 404 Resource not found
        }
        return Ok(box); // 200 OK with customer in body
    }

    // POST: api/boxes
    // BODY: Box (JSON, XML)
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(BoxEfModel))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] BoxEfModel box)
    {
        if (box == null)
        {
            return BadRequest(); // 400 Bad request
        }

        BoxEfModel? addedBox = await repo.CreateAsync(box);

        if (addedBox == null)
        {
            return BadRequest("Repository failed to create box.");
        }
        else
        {
            return CreatedAtRoute( // 201 Created
                routeName: nameof(GetBox),
                routeValues: new { id = addedBox.Id },
                value: addedBox);
        }
    }

    // PUT: api/boxes/[id]
    // BODY: Box (JSON, XML)
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(string id, [FromBody] BoxEfModel box)
    {
        if (box == null || box.Id != new Guid(id))
        {
            return BadRequest(); // 400 Bad request
        }

        BoxEfModel? existing = await repo.RetrieveAsync(id);
        if (existing == null)
        {
            return NotFound(); // 404 Resource not found
        }

        await repo.UpdateAsync(id, box);

        return new NoContentResult(); // 204 No content
    }

    // DELETE: api/boxes/[id]
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(string id)
    {
        BoxEfModel? existing = await repo.RetrieveAsync(id);

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
