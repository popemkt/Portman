using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portman.Domain;
using Portman.Domain.Entities;

namespace Portman.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShipsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ShipsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ship>>> GetShips()
    {
        return Ok(await _unitOfWork.ShipRepository.GetAll().ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Ship>> GetShip(string id)
    {
        var ship = await _unitOfWork.ShipRepository.GetByIdAsync(id);

        if (ship == null)
        {
            return NotFound();
        }

        return ship;
    }

    [HttpPost]
    public async Task<ActionResult<Ship>> CreateShip(Ship ship)
    {
        _unitOfWork.ShipRepository.Insert(ship);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetShip), new { id = ship.Id }, ship);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShip(string id, Ship ship)
    {
        if (id != ship.Id)
        {
            return BadRequest();
        }

        _unitOfWork.ShipRepository.Update(ship);

        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException) when (!ShipExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    private bool ShipExists(string id)
    {
        return _unitOfWork.ShipRepository.GetAll().Any(e => e.Id == id);
    }
}