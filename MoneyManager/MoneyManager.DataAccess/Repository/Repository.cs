using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly MoneyManagerDataContext _dataContext;
        private readonly DbSet<T> _entities;
        public Repository(MoneyManagerDataContext dataContext)
        {
            _dataContext = dataContext;
            _entities = _dataContext.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> entities = _entities;

            entities = entities.Where(filter);

            return await entities.FirstOrDefaultAsync();
        }
        public void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _entities.RemoveRange(entities);
        }
    }
}
