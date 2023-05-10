
using NetTopologySuite.Geometries;

namespace Portman.Domain.Entities;

public class Port
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Point Location { get; set; } 
}