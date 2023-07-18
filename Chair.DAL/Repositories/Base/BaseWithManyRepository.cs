using Chair.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chair.DAL.Data.Entities;
using System.Linq.Expressions;

namespace Chair.DAL.Repositories.Base;

public abstract class BaseWithManyRepository<T> : IBaseWithManyRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _dbContext;
    protected DbSet<T> _dbSet;

    protected BaseWithManyRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
        => await _dbSet.FindAsync(id);

    public virtual IQueryable<T> GetAllAsync()
        => _dbSet.AsQueryable();

    public virtual IQueryable<T> GetAllByPredicateAsQueryable(Expression<Func<T, bool>>? predicate = null)
    {
        return predicate == null ? _dbSet : _dbSet.Where(predicate);
    }

    public virtual async Task AddAsync(T model)
    {
        await _dbSet.AddAsync(model);
        await _dbContext.SaveChangesAsync();
    }
    public virtual async Task AddManyAsync(List<T> models)
    {
        await _dbSet.AddRangeAsync(models);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(T model)
    {
        _dbSet.Update(model);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task UpdateManyAsync(List<T> models)
    {
        _dbSet.UpdateRange(models);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task RemoveAsync(T model)
    {
        _dbSet.Remove(model);
        await _dbContext.SaveChangesAsync();
    }
    
    public virtual async Task RemoveManyAsync(List<T> models)
    {
        _dbSet.RemoveRange(models);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task RemoveByIdAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if(entity == null)
            throw new ArgumentNullException($"object with this id = {id} not found");
        await RemoveAsync(entity);
        await _dbContext.SaveChangesAsync();
    }
    
    public virtual async Task RemoveManyByIdsAsync(List<Guid> ids)
    {
        var entities = _dbSet.Where(x=>ids.Contains(x.Id));
        if(entities == null || entities.Count() < 0)
            throw new ArgumentNullException($"object with this id = {string.Join(",", ids)} not found");
        await RemoveManyAsync(await entities.ToListAsync());
        await _dbContext.SaveChangesAsync();
    }
}
