using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Repository
{
    public interface IUnitOfWork
    {
        IBoughtProductReposiotry BoughtProduct { get; set; }
        Task SaveAsync();
    }
}
