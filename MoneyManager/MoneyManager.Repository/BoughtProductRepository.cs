﻿using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Repository
{
	public class BoughtProductRepository : Repository<BoughtProduct>, IBoughtProductReposiotry
	{
		private readonly MoneyManagerContext _dbContext;
		public BoughtProductRepository(MoneyManagerContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
		public void Update(BoughtProduct boughtProduct)
		{
            _dbContext.BoughtProducts.Update(boughtProduct);
        }
	}
}