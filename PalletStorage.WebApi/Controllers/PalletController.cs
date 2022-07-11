﻿using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PalletStorage.Common.CommonClasses;
using PalletStorage.Repositories.Repositories;
using PalletStorage.WebApi.Models.Models;

namespace PalletStorage.WebApi.Controllers;

// base address: api/pallets
[Route("api/[controller]")]
[ApiController]
public class PalletController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IValidator<PalletApiModel> validator;
    private readonly IPalletRepository repo;

    // constructor injects repository registered in Startup
    public PalletController(IPalletRepository repo, IMapper mapper, IValidator<PalletApiModel> validator)
    {
        this.repo = repo;
        this.mapper = mapper;
        this.validator = validator;
    }

    // GET: api/pallets
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<PalletApiModel>))]
    public async Task<IEnumerable<PalletApiModel>> GetPallets(int take = 100, int skip = 0)
    {
        var pallets = await repo.GetAllAsync(take, skip);

        return pallets.Select(p => mapper.Map<PalletApiModel>(p)).AsEnumerable();
    }

    // GET: api/pallets/[id]
    [HttpGet("{id:int}", Name = nameof(GetPallet))] // named route
    [ProducesResponseType(200, Type = typeof(PalletApiModel))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetPallet(int id)
    {
        Pallet? pallet = await repo.GetAsync(id);

        if (pallet == null)
        {
            return NotFound(); // 404 Resource not found
        }

        return Ok(mapper.Map<PalletApiModel>(pallet)); // 200 OK with customer in body
    }

    // POST: api/pallets
    // BODY: pallets (JSON, XML)
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(PalletApiModel))]
    [ProducesResponseType(400)]
    public async Task<ActionResult<PalletApiModel>> Create([FromBody] PalletApiModel pallet)
    {
        var result = await validator.ValidateAsync(pallet);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors.ToList()); // 400 Bad request
        }

        var addedPallet = await repo.CreateAsync(mapper.Map<Pallet>(pallet));

        if (addedPallet == null)
        {
            return BadRequest("Repository failed to create new pallet.");
        }

        return mapper.Map<PalletApiModel>(addedPallet);
    }

    // PUT: api/pallets
    // BODY: Pallet (JSON, XML)
    [HttpPut("{pallet}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update([FromBody] PalletApiModel pallet)
    {
        if (pallet.Id == null)
        {
            return BadRequest(); // 400 Bad request
        }

        var result = await validator.ValidateAsync(pallet);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors.ToList()); // 400 Bad request
        }

        var existing = await repo.UpdateAsync(mapper.Map<Pallet>(pallet));

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
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await repo.DeleteAsync(id);

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
    public async Task<IActionResult> AddBoxToPallet([FromBody] BoxApiModel box, int palletId)
    {
        if (box.Id == null)
        {
            return BadRequest(); // 400 Bad request
        }

        var existing = await repo.AddBoxToPalletAsync(mapper.Map<Box>(box), palletId);

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
        if (box.Id == null)
        {
            return BadRequest(); // 400 Bad request
        }

        var existing = await repo.DeleteBoxFromPalletAsync(mapper.Map<Box>(box));

        if (existing == null)
        {
            return NotFound(); // 404 Resource not found
        }

        return new NoContentResult(); // 204 No content
    }
}
