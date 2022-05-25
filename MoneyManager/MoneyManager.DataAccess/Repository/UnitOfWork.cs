using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MoneyManagerDataContext _dataContext;
        public UnitOfWork(MoneyManagerDataContext dataContext)
        {
            _dataContext = dataContext;
            Category = new CategoryRepository(_dataContext);
        }
        public ICategoryRepository Category { get; set; }
        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
