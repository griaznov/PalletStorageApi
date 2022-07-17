using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FluentValidation;
using PalletStorage.Business.Models;
using PalletStorage.WebApi.Models.Models.Box;
using PalletStorage.Repositories;

namespace PalletStorage.WebApi.Controllers;

// base address: api/boxes
[Route("api/[controller]")]
[ApiController]
public class BoxController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IValidator<BoxCreateRequest> createValidator;
    private readonly IValidator<BoxUpdateRequest> updateValidator;
    private readonly IBoxRepository repo;

    // constructor injects repository registered in Startup
    public BoxController(IBoxRepository repo,
        IMapper mapper,
        IValidator<BoxCreateRequest> createValidator,
        IValidator<BoxUpdateRequest> updateValidator)
    {
        this.repo = repo;
        this.mapper = mapper;
        this.createValidator = createValidator;
        this.updateValidator = updateValidator;
    }

    // GET: api/boxes
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<BoxResponse>))]
    public async Task<IEnumerable<BoxResponse>> GetBoxes(int take = 100, int skip = 0)
    {
        var boxes = await repo.GetAllAsync(take, skip);

        return boxes.Select(box => mapper.Map<BoxResponse>(box)).AsEnumerable();
    }

    // GET: api/boxes/[id]
    [HttpGet("{id:int}", Name = nameof(GetBox))] // named route
    [ProducesResponseType(200, Type = typeof(BoxResponse))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetBox(int id)
    {
        BoxModel? box = await repo.GetAsync(id);

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
    public async Task<ActionResult<BoxResponse>> Create([FromBody] BoxCreateRequest box)
    {
        var result = await createValidator.ValidateAsync(box);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors.ToList()); // 400 Bad request
        }

        var addedBox = await repo.CreateAsync(mapper.Map<BoxModel>(box));

        if (addedBox == null)
        {
            return BadRequest("Repository failed to create box.");
        }

        return mapper.Map<BoxResponse>(addedBox);
    }

    // PUT: api/boxes
    // BODY: Box (JSON, XML)
    [HttpPut("{box}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update([FromBody] BoxUpdateRequest box)
    {
        var result = await updateValidator.ValidateAsync(box);

        if (!result.IsValid)
        {
            return BadRequest(result.Errors.ToList()); // 400 Bad request
        }

        var existing = await repo.UpdateAsync(mapper.Map<BoxModel>(box));

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
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await repo.DeleteAsync(id);

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
