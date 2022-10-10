using MoneyManager.Models;

namespace MoneyManager.Repository
{
    public class IncomeRepository : Repository<Income>, IIncomeRepository
    {
        private readonly MoneyManagerContext _dbContext;
        public IncomeRepository(MoneyManagerContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(Income item)
        {
            _dbContext.Incomes.Update(item);
        }
    }
}