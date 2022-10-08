namespace MoneyManager.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MoneyManagerContext _dataContext;
        public UnitOfWork(MoneyManagerContext dataContext)
        {
            _dataContext = dataContext;
            BoughtProduct = new BoughtProductRepository(_dataContext);
        }
        public IBoughtProductReposiotry BoughtProduct { get; set; }
        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
