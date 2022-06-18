using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.DataAccess.Repository
{
	public class BoughtProductRepository : Repository<BoughtProduct>, IBoughtProductReposiotry
	{
		private readonly MoneyManagerDataContext _dbContext;
		public BoughtProductRepository(MoneyManagerDataContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
		public void Update(BoughtProduct boughtProduct)
		{
            _dbContext.BoughtProducts.Update(boughtProduct);
        }
	}
}
