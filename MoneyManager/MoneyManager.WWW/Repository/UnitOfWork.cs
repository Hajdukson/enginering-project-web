using MoneyManager.DataAccess.Repository;
using MoneyManager.WWW.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MoneyManagerWWWContext _dataContext;
        public UnitOfWork(MoneyManagerWWWContext dataContext)
        {
            _dataContext = dataContext;
            Category = new CategoryRepository(_dataContext);
            Product = new ProductRepository(_dataContext);
            BoughtProduct = new BoughtProductRepository(_dataContext);
        }
        public ICategoryRepository Category { get; set; }
        public IProductRepository Product { get; set; }
        public IBoughtProductReposiotry BoughtProduct { get; set; }
        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
