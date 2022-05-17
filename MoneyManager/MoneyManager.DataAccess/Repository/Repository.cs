using MoneyManager.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public Repository()
        {

        }
        public void Add(T entity)
        {
            
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public void Remove(T entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            throw new NotImplementedException();
        }
    }
}
