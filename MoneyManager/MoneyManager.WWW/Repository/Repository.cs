using Microsoft.EntityFrameworkCore;
using MoneyManager.WWW.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        #region CTOR
        private readonly MoneyManagerContext _dataContext;
        private readonly DbSet<T> _entities;
        public Repository(MoneyManagerContext dataContext)
        {
            _dataContext = dataContext;
            _entities = _dataContext.Set<T>();
        }
        #endregion
        #region ASYNC METHODS
        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
        }
        public async Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null)
        {
            IQueryable<T> entities = _entities;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                IncludeProperties(ref entities, includeProperties);
            }
            return await entities.ToListAsync();
        }
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> entities = _entities;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                IncludeProperties(ref entities, includeProperties);
            }
            entities = entities.Where(filter);

            return await entities.FirstOrDefaultAsync();
        }
        #endregion
        #region SYNC METHODS
        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> entities = _entities;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                IncludeProperties(ref entities, includeProperties);
            }
            return entities.ToList();
        }
        public void Remove(T entity)
        {
            _entities.Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _entities.RemoveRange(entities);
        }
        #endregion
        private void IncludeProperties(ref IQueryable<T> entities, string includeProperties)
        {
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }))
            {
                entities = entities.Include(includeProperty);
            }
        }
    }
}