using Portman.Domain.Entities;

namespace Portman.Domain;

public interface IUnitOfWork : IDisposable
{
    IRepository<Ship> ShipRepository { get; }
    IRepository<Port> PortRepository { get; }
    Task<int> CompleteAsync();
}