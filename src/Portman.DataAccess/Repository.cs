using Microsoft.EntityFrameworkCore;
using Portman.Domain;

namespace Portman.DataAccess;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly PortFinderContext Context;
    internal DbSet<T> dbSet;

    public Repository(PortFinderContext context)
    {
        Context = context;
        this.dbSet = context.Set<T>();
    }

    public IQueryable<T> GetAll()
    {
        return dbSet;
    }

    public async Task<T> GetByIdAsync(object id)
    {
        return await dbSet.FindAsync(id);
    }

    public void Insert(T obj)
    {
        dbSet.Add(obj);
    }

    public void Update(T obj)
    {
        dbSet.Attach(obj);
        Context.Entry(obj).State = EntityState.Modified;
    }

    public async Task DeleteAsync(object id)
    {
        T entityToDelete = await dbSet.FindAsync(id);
        Delete(entityToDelete);
    }

    public void Delete(T entityToDelete)
    {
        if (Context.Entry(entityToDelete).State == EntityState.Detached)
        {
            dbSet.Attach(entityToDelete);
        }

        dbSet.Remove(entityToDelete);
    }

    public async Task SaveAsync()
    {
        await Context.SaveChangesAsync();
    }
}