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
        }
        public async Task Save()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
