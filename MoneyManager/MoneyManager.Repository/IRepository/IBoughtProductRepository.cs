using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Repository
{
	public interface IBoughtProductRepository : IRepository<BoughtProduct>
	{
		void Update(BoughtProduct boughtProduct);
		Task<List<ProductSummary>> GetBoughtProductsSummaries(
			string? name, 
			DateTime? startDate = null, 
			DateTime? endDate = null);
        List<BoughtProduct> DeleteProductsByName(IEnumerable<string> names);
        IQueryable<BoughtProduct> GetProductsFromConcreteReceipt(string names);
    }
}
