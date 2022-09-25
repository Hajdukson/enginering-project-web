using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.DataAccess
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; set; }
        IProductRepository Product { get; set; }
        IBoughtProductReposiotry BoughtProduct { get; set; }
        Task SaveAsync();
    }
}
