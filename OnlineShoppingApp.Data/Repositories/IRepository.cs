using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Data.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity, bool softDelete = true);
        Task DeleteAsync(int id);
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity, bool softDelete = true);
        void Delete(int id);
        TEntity GetById(int id);
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null);
    }
}
