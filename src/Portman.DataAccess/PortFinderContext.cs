using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using Portman.Domain.Entities;
using Coordinate = NetTopologySuite.Geometries.Coordinate;

namespace Portman.DataAccess;

public class PortFinderContext : DbContext
{
    public PortFinderContext(DbContextOptions<PortFinderContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        // Seed 10 ships
        for (int i = 0; i < 10; i++)
        {
            modelBuilder.Entity<Ship>().HasData(new Ship
            {
                Id = (i + 1).ToString(),
                Name = $"Ship{i + 1}",
                Location = geometryFactory.CreatePoint(new Coordinate(i + 10, i + 20)),
                Velocity = 20
            });
        }

        // Seed 10 ports
        for (int i = 0; i < 10; i++)
        {
            modelBuilder.Entity<Port>().HasData(new Port
            {
                Id = (i + 1).ToString(),
                Name = $"Port{i + 1}",
                Location = geometryFactory.CreatePoint(new Coordinate(i + 50, i + 60))
            });
        }
    }

    public DbSet<Ship> Ships { get; set; }
    public DbSet<Port> Ports { get; set; }
}