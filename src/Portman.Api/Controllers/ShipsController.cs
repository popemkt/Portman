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

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateShip(Guid id, Ship ship)
    {
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

    private bool ShipExists(Guid id)
    {
        return _unitOfWork.ShipRepository.GetAll().Any(e => e.Id == id);
    }
}