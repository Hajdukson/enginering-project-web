using Microsoft.EntityFrameworkCore;
using MoneyManager.Models;

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

		public async Task<List<ProductSummary>> GetBoughtProductsSummaries(DateTime? startDate = null, DateTime? endDate = null)
		{
			IQueryable<BoughtProduct> products;

            if (startDate != null || endDate != null)
			{
                products  = _dbContext.BoughtProducts.Where(bp => bp.BoughtDate >= startDate && bp.BoughtDate <= endDate);
            }
			else
			{
				products = _dbContext.BoughtProducts;
            }
			
			var distincProductsByName = products
				.GroupBy(p => p.Name)
				.Select(bp => bp.First());

			var productsSummaries = new List<ProductSummary>();

			foreach (var product in distincProductsByName)
			{
				var singleProducts = await products
					.Where(bp => bp.Name == product.Name)
					.OrderBy(bp => bp.BoughtDate)
					.ToListAsync();

                var startProduct = singleProducts[0];
                var endProduct = singleProducts[singleProducts.Count - 1];

                var productSummary = new ProductSummary()
				{
					Name = product.Name,
					StartProduct = startProduct,
					EndProduct = endProduct,
					Inflation = Math.Round((endProduct.Price / startProduct.Price - 1), 2) * 100,
				};

				productsSummaries.Add(productSummary);
			}

			return productsSummaries;
        }
	}
}
