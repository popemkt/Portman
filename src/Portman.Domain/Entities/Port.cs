
using NetTopologySuite.Geometries;

namespace Portman.Domain.Entities;

public class Port
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Point Location { get; set; } 
}