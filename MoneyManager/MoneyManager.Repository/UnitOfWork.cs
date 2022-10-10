using MoneyManager.Models;
using MoneyManager.Repository.IRepository;

namespace MoneyManager.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MoneyManagerContext _dataContext;
        public UnitOfWork(MoneyManagerContext dataContext)
        {
            _dataContext = dataContext;
            BoughtProduct = new BoughtProductRepository(_dataContext);
            Items = new ItemRepository(_dataContext);
            Outcomes = new OutcomeRepository(_dataContext);
            Incomes = new IncomeRepository(_dataContext);
            ApplicationUsers = new ApplicationUserRepository(_dataContext);
            Categories = new CategoryRepository(_dataContext);
            
        }
        public IBoughtProductReposiotry BoughtProduct { get; private set; }
        public IItemRepository Items { get; private set; }
        public IOutcomeRepository Outcomes { get; private set; }
        public IIncomeRepository Incomes { get; private set; }
        public IApplicationUserRepository ApplicationUsers { get; private set; }
        public ICategoryRepository Categories { get; private set; }

        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
