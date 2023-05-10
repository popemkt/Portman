using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using Portman.Api.Controllers;
using Portman.DataAccess;
using Portman.Domain;
using Portman.Domain.Entities;
using Xunit;

namespace Portman.IntegrationTests;

public class PortsControllerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly PortsController _controller;

    public PortsControllerTests()
    {
        var options = new DbContextOptionsBuilder<PortFinderContext>()
            .UseInMemoryDatabase(databaseName: "PortFinder")
            .Options;
        var context = new PortFinderContext(options);
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _controller = new PortsController(context, _unitOfWorkMock.Object);
    }

    // Testing GetClosestPort
    [Fact]
    public async Task GetClosestPort_ShouldReturnClosestPortAndEstimatedArrivalTime()
    {
        // Arrange
        var shipId = Guid.NewGuid().ToString();
        var ship = new Ship { Id = Guid.Parse(shipId), Name = "Ship1", Velocity = 10 };

        _unitOfWorkMock.Setup(x => x.ShipRepository.GetByIdAsync(shipId)).ReturnsAsync(ship);

        // Act
        var result = await _controller.GetClosestPort(shipId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        var returnPort = Assert.IsAssignableFrom<ClosestPortResult>(okResult.Value);
    }
    
    private class ClosestPortResult
    {
        public Port Port { get; set; }
        public double EstimatedArrivalTime { get; set; }
    }
}

