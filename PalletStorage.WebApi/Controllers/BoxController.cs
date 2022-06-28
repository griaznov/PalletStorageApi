﻿using Microsoft.AspNetCore.Mvc;
using PalletStorage.Common.CommonClasses;
using PalletStorage.Repositories.Repositories;
using PalletStorage.WebApi.Models.Converters;
using PalletStorage.WebApi.Models.Models;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;

namespace PalletStorage.WebApi.Controllers;

// base address: api/boxes
[Route("api/[controller]")]
[ApiController]
public class BoxesController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IValidator<BoxApiModel> validator;
    private readonly IBoxRepository repo;

    // constructor injects repository registered in Startup
    public BoxesController(IBoxRepository repo, IMapper mapper, IValidator<BoxApiModel> validator)
    {
        this.repo = repo;
        this.mapper = mapper;
        this.validator = validator;
    }

    // GET: api/boxes
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<BoxApiModel>))]
    public async Task<IEnumerable<BoxApiModel>> GetBoxes(int count = 0, int skip = 0)
    {
        var boxes = await repo.RetrieveAllAsync(count, skip);

        return boxes.Select(box => mapper.Map<BoxApiModel>(box)).AsEnumerable();
    }

    // GET: api/boxes/[id]
    [HttpGet("{id:int}", Name = nameof(GetBox))] // named route
    [ProducesResponseType(200, Type = typeof(BoxApiModel))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetBox(int id)
    {
        Box? box = await repo.RetrieveAsync(id);

        if (box == null)
        {
            return NotFound(); // 404 Resource not found
        }

        return Ok(mapper.Map<BoxApiModel>(box)); // 200 OK with customer in body
    }

    // POST: api/boxes
    // BODY: Box (JSON, XML)
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(BoxApiModel))]
    [ProducesResponseType(400)]
    public async Task<ActionResult<BoxApiModel>> Create([FromBody] BoxApiModel box)
    {
        if (box == null)
        {
            return BadRequest("Wrong model for the box."); // 400 Bad request
        }

        var result = await validator.ValidateAsync(box);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors.ToList()); // 400 Bad request
        }

        var addedBox = await repo.CreateAsync(mapper.Map<Box>(box));

        if (addedBox == null)
        {
            return BadRequest("Repository failed to create box.");
        }

        //return CreatedAtRoute( // 201 Created
        //    routeName: nameof(GetBox),
        //    routeValues: new { id = addedBox.Id },
        //    value: addedBox);
        return mapper.Map<BoxApiModel>(addedBox);
    }

    // PUT: api/boxes
    // BODY: Box (JSON, XML)
    [HttpPut("{box}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update([FromBody] BoxApiModel box)
    {
        if (box?.Id == null)
        {
            return BadRequest("Wrong model or ID for the box."); // 400 Bad request
        }

        var result = await validator.ValidateAsync(box);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors.ToList()); // 400 Bad request
        }

        var existing = await repo.UpdateAsync(mapper.Map<Box>(box));

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
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await repo.DeleteAsync(id);

        if (deleted == null)
        {
            return NotFound(); // 404 Resource not found
        }

        if (deleted.Value)
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
