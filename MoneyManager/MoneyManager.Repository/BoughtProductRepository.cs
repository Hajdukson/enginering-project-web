using Microsoft.EntityFrameworkCore;
using MoneyManager.Models;
using System.Data.Common;
using System.Drawing.Printing;

namespace MoneyManager.Repository
{
	public class BoughtProductRepository : Repository<BoughtProduct>, IBoughtProductRepository
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

		public async Task<List<ProductSummary>> GetBoughtProductsSummaries(string? name, DateTime? startDate, DateTime? endDate)
		{
			IQueryable<BoughtProduct> products;

            if (startDate != null && endDate != null)
			{
                products  = _dbContext.BoughtProducts.Where(bp => bp.BoughtDate >= startDate && bp.BoughtDate <= endDate);
            }
			else if(startDate != null)
			{
				products = _dbContext.BoughtProducts.Where(bp => bp.BoughtDate >= startDate);
			}
			else if(endDate != null)
			{
                products = _dbContext.BoughtProducts.Where(bp => bp.BoughtDate <= endDate);
            }
			else
			{
				products = _dbContext.BoughtProducts;
            }

			if(!string.IsNullOrEmpty(name))
			{
				products = products.Where(bp => bp.Name.ToLower().Contains(name.ToLower()));
			}
            await Console.Out.WriteLineAsync("Executing query...");
            var distinctProductsByName = products
				.GroupBy(p => p.Name)
				.Select(bp => bp.First());

            await Console.Out.WriteLineAsync(distinctProductsByName.ToQueryString());

            var productsSummaries = new List<ProductSummary>();

			foreach (var product in distinctProductsByName)
			{
				var singleProducts = await products
					.Where(bp => bp.Name == product.Name)
					.OrderBy(bp => bp.BoughtDate)
					.ToListAsync();

                var startProduct = singleProducts[0];
                var endProduct = singleProducts[singleProducts.Count - 1];
                var endPrice = endProduct.Price;
                var startPrice = startProduct.Price == 0 ? 1 : startProduct.Price;

                var productSummary = new ProductSummary()
				{
					Name = product.Name,
					StartProduct = startProduct,
					EndProduct = endProduct,
					Inflation = Math.Round((endPrice / startPrice - 1), 2) * 100,
				};

				productsSummaries.Add(productSummary);
			}

			return productsSummaries;
        }

		public List<BoughtProduct> DeleteProductsByName(IEnumerable<string> names)
		{
			var deletedProducts = new List<BoughtProduct>();
			if (names != null && names.Count() > 0)
			{
                foreach (var name in names)
                {
                    var products = _dbContext.BoughtProducts.Where(c => c.Name == name);

                    if (products.Count() > 0)
                    {
                        this.RemoveRange(products);
                        deletedProducts.AddRange(products);
                    }

                }
            }
			else
			{
				return new List<BoughtProduct>();
			}

            return deletedProducts;
        }

		public IQueryable<BoughtProduct> GetProductsFromConcreteReceipt(string imagePath)
		{
			var productsFromConcreteReceipt = _dbContext.BoughtProducts.Where(bp => bp.ImagePath == imagePath);
			return productsFromConcreteReceipt;
		}
    }
}
