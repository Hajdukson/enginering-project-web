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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly MoneyManagerContext _dbContext;
        public CategoryRepository(MoneyManagerContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(Category category)
        {
            _dbContext.Categories.Update(category);
        }
    }
}
