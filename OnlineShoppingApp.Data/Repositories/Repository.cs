using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Data.Context;
using OnlineShoppingApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly OnlineShoppingAppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(OnlineShoppingAppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(TEntity entity, bool softDelete = true)
        {
            if (softDelete && entity is BaseEntity baseEntity)
            {
                baseEntity.IsDeleted = true;
                await UpdateAsync(entity);
            }
            else
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }
        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null ? await _dbSet.ToListAsync() : await _dbSet.Where(predicate).ToListAsync();
        }
        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }
        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }
        public void Delete(TEntity entity, bool softDelete = true)
        {
            if (softDelete && entity is BaseEntity baseEntity)
            {
                baseEntity.IsDeleted = true;
                Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }
        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }
        public TEntity GetById(int id)
        {
            return _dbSet.Find(id);
        }
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }
        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null ? _dbSet : _dbSet.Where(predicate);
        }
    }
}
