using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MoneyManager.Repository
{
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
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _entities.AddRangeAsync(entities);
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
        public IQueryable<T> GetAll(IEnumerable<Expression<Func<T, bool>>>? filters = null, string ? includeProperties = null)
        {
            IQueryable<T> entities = _entities;

            if(filters != null)
            {
                foreach (var filter in filters)
                {
                    if(filter != null)
                        entities = entities.Where(filter);
                }
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                IncludeProperties(ref entities, includeProperties);
            }

            return entities;
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