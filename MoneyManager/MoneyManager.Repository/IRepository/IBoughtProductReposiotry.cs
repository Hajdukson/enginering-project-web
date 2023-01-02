using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Repository
{
	public interface IBoughtProductReposiotry : IRepository<BoughtProduct>
	{
		void Update(BoughtProduct boughtProduct);
		Task<List<ProductSummary>> GetBoughtProductsSummaries(DateTime? startDate = null, DateTime? endDate = null);

    }
}
