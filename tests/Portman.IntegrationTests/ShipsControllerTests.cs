using Microsoft.AspNetCore.Mvc;
using Moq;
using Portman.Api.Controllers;
using Portman.Domain;
using Portman.Domain.Entities;
using Xunit;

namespace Portman.IntegrationTests;

public class ShipsControllerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ShipsController _controller;

    public ShipsControllerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _controller = new ShipsController(_unitOfWorkMock.Object);
    }

    // Testing GetShips
    [Fact]
    public async Task GetShips_ShouldReturnAllShips()
    {
        // Arrange
        var ships = new List<Ship>
        {
            new Ship { Id = Guid.NewGuid(), Name = "Ship1", Velocity = 10 },
            new Ship { Id = Guid.NewGuid(), Name = "Ship2", Velocity = 15 }
        }.AsQueryable();

        _unitOfWorkMock.Setup(x => x.ShipRepository.GetAll()).Returns(ships);

        // Act
        var result = await _controller.GetShips();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnShips = Assert.IsAssignableFrom<IEnumerable<Ship>>(okResult.Value);
        Assert.Equal(2, returnShips.Count());
    }
}
