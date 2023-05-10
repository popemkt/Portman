namespace Portman.Domain;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetAll();
    Task<T> GetByIdAsync(object id);
    void Insert(T obj);
    void Update(T obj);
    Task DeleteAsync(object id);
    Task SaveAsync();
}