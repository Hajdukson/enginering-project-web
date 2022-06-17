using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.DataAccess
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        IEnumerable<T> GetAll();
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
