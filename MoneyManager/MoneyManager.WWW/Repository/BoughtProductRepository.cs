using MoneyManager.Models;
using MoneyManager.WWW.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.DataAccess.Repository
{
	public class BoughtProductRepository : Repository<BoughtProduct>, IBoughtProductReposiotry
	{
		private readonly MoneyManagerWWWContext _dbContext;
		public BoughtProductRepository(MoneyManagerWWWContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
		public void Update(BoughtProduct boughtProduct)
		{
            _dbContext.BoughtProducts.Update(boughtProduct);
        }
	}
}
