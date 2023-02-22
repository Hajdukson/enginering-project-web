﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter, string? includeProperties = null);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        IQueryable<T> GetAll(IEnumerable<Expression<Func<T, bool>>> filters = null, string? includeProperties = null);
        T Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
