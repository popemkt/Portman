using Portman.Domain;
using Portman.Domain.Entities;

namespace Portman.DataAccess;


public class UnitOfWork : IUnitOfWork
{
    private readonly PortFinderContext _context;
    private bool disposed = false;

    public UnitOfWork(PortFinderContext context)
    {
        _context = context;
        ShipRepository = new Repository<Ship>(_context);
        PortRepository = new Repository<Port>(_context);
    }

    public IRepository<Ship> ShipRepository { get; private set; }

    public IRepository<Port> PortRepository { get; private set; }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}