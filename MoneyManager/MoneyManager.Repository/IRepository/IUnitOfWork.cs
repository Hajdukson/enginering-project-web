using MoneyManager.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Repository
{
    public interface IUnitOfWork
    {
        IBoughtProductRepository BoughtProduct { get; }
        IItemRepository Items { get; }
        IOutcomeRepository Outcomes { get; }
        IIncomeRepository Incomes { get; }
        IApplicationUserRepository ApplicationUsers { get; }
        ICategoryRepository Categories { get; }
        Task SaveAsync();
    }
}
