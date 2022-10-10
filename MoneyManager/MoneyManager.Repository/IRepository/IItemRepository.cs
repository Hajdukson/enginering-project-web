using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Repository.IRepository
{
    public interface IItemRepository : IRepository<Item>
    {
        void Update(Item item);
    }
}
