using MoneyManager.DataAccess;
using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.DataAccess
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly MoneyManagerDataContext _dbContext;
        public CategoryRepository(MoneyManagerDataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(Category category)
        {
            _dbContext.Categories.Update(category);
        }
    }
}
