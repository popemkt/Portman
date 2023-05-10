using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portman.DataAccess;
using Portman.Domain;

namespace Portman.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortsController : ControllerBase
{
    private readonly PortFinderContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public PortsController(PortFinderContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("{shipId}")]
    public async Task<ActionResult> GetClosestPort(string shipId)
    {
        var ship = await _unitOfWork.ShipRepository.GetByIdAsync(shipId);
        if (ship == null)
        {
            return NotFound();
        }

        // Query the nearest port using PostGIS functions
        var nearestPort = await _context.Ports
            .OrderBy(p => p.Location.Distance(ship.Location))
            .FirstOrDefaultAsync();

        // If no port was found, return not found
        if (nearestPort == null)
        {
            return NotFound();
        }

        // Calculate the estimated arrival time
        var distance = ship.Location.Distance(nearestPort.Location);
        var estimatedArrivalTime = distance / ship.Velocity;

        return Ok(new
        {
            Port = nearestPort,
            EstimatedArrivalTime = estimatedArrivalTime
        });
    }
}