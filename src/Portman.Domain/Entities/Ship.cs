using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace Portman.Domain.Entities;

public class Ship
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Point Location { get; set; } 
    public double Velocity { get; set; }
}