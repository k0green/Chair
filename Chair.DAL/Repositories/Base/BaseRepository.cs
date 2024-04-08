using Chair.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Chair.DAL.Data.Entities;
using System.Linq.Expressions;

namespace Chair.DAL.Repositories.Base;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _dbContext;
    protected DbSet<T> _dbSet;

    public BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
        => await _dbSet.FindAsync(id);

    public virtual IQueryable<T> GetAllAsync()
        => _dbSet.AsQueryable();

    public IQueryable<T> GetAllByPredicateAsQueryable(Expression<Func<T, bool>>? predicate = null)
    {
        return predicate == null ? _dbSet : _dbSet.Where(predicate);
    }

    public virtual async Task AddAsync(T model)
    {
        await _dbSet.AddAsync(model);
    }

    public virtual async Task UpdateAsync(T model)
    {
        _dbSet.Update(model);
    }

    public virtual async Task RemoveAsync(T model)
    {
        _dbSet.Remove(model);
    }

    public virtual async Task RemoveByIdAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
            throw new ArgumentNullException($"object with this id = {id} not found");
        await RemoveAsync(entity);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}
