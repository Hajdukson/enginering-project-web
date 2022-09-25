using MoneyManager.DataAccess;
using MoneyManager.Models;
using MoneyManager.WWW.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.DataAccess
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly MoneyManagerWWWContext _dbContext;
        public ProductRepository(MoneyManagerWWWContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(Product product)
        {
            _dbContext.Products.Update(product);
        }
    }
}
