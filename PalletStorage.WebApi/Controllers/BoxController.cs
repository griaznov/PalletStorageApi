using Microsoft.AspNetCore.Mvc;
using PalletStorage.Common.CommonClasses;
using PalletStorage.Repositories.Repositories;
using PalletStorage.WebApi.Models.Converters;
using PalletStorage.WebApi.Models.Models;

namespace PalletStorage.WebApi.Controllers;

// base address: api/boxes
[Route("api/[controller]")]
[ApiController]
public class BoxesController : ControllerBase
{
    //private readonly delIBoxRepository repo;
    private readonly IBoxRepository repo;

    // constructor injects repository registered in Startup
    public BoxesController(IBoxRepository repo)
    {
        this.repo = repo;
        //this.repoCommon = repo;
    }

    // GET: api/boxes
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<BoxApiModel>))]
    public async Task<IEnumerable<BoxApiModel>> GetBoxes()
    {
        var boxes = await repo.RetrieveAllAsync();

        return boxes.Select(box => box.ToApiModel()).AsEnumerable();
    }

    // GET: api/boxes/[id]
    [HttpGet("{id:int}", Name = nameof(GetBox))] // named route
    [ProducesResponseType(200, Type = typeof(BoxApiModel))]
    [ProducesResponseType(404)]
    //public async Task<IActionResult> GetBox(string id)
    public async Task<IActionResult> GetBox(int id)
    {
        Box? box = await repo.RetrieveAsync(id);

        if (box == null)
        {
            return NotFound(); // 404 Resource not found
        }

        return Ok(box.ToApiModel()); // 200 OK with customer in body
    }

    // POST: api/boxes
    // BODY: Box (JSON, XML)
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(BoxApiModel))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] BoxApiModel box)
    {
        if (box == null)
        {
            return BadRequest("Wrong model for the box."); // 400 Bad request
        }

        Box? addedBox = await repo.CreateAsync(box.ToCommonModel());

        if (addedBox == null)
        {
            return BadRequest("Repository failed to create box.");
        }

        return CreatedAtRoute( // 201 Created
            routeName: nameof(GetBox),
            routeValues: new { id = addedBox.Id },
            value: addedBox);
    }

    // PUT: api/boxes/[id]
    // BODY: Box (JSON, XML)
    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] BoxApiModel box)
    {
        if (box == null || box.Id != id)
        {
            return BadRequest("Wrong model or ID for the box."); // 400 Bad request
        }

        Box? existing = await repo.RetrieveAsync(id);

        if (existing == null)
        {
            return NotFound(); // 404 Resource not found
        }

        await repo.UpdateAsync(box.ToCommonModel());

        return new NoContentResult(); // 204 No content
    }

    // DELETE: api/boxes/[id]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        Box? existing = await repo.RetrieveAsync(id);

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
                $"Box {id} was found but failed to delete.");
        }
    }

}
