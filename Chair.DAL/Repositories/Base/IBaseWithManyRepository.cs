using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Chair.DAL.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chair.DAL.Repositories.Base
{
    public interface IBaseWithManyRepository<T> where T : BaseEntity
    {
        public Task<T?> GetByIdAsync(Guid id);
        public IQueryable<T> GetAllAsync();
        public IQueryable<T> GetAllByPredicateAsQueryable(Expression<Func<T, bool>>? predicate = null);

        public Task AddAsync(T model);
        public Task AddManyAsync(List<T> models);

        public Task UpdateAsync(T model);
        public Task UpdateManyAsync(List<T> models);

        public Task RemoveAsync(T model);
        public Task RemoveManyAsync(List<T> models);

        public Task RemoveByIdAsync(Guid id);
        public Task RemoveManyByIdsAsync(List<Guid> ids);
    }
}
