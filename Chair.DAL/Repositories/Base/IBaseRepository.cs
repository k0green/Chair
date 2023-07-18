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
    public interface IBaseRepository<T> where T : BaseEntity
    {
        public Task<T?> GetByIdAsync(Guid id);
        public IQueryable<T> GetAllAsync();
        public IQueryable<T> GetAllByPredicateAsQueryable(Expression<Func<T, bool>>? predicate = null);

        public Task AddAsync(T model);
        public Task UpdateAsync(T model);

        public Task RemoveAsync(T model);
        public Task RemoveByIdAsync(Guid id);
    }
}
