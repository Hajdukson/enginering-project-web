using MoneyManager.Models;
using MoneyManager.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly MoneyManagerContext _dataContext;
        public ApplicationUserRepository(MoneyManagerContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }

        public void Update(ApplicationUser user)
        {
            _dataContext.ApplicationUsers.Update(user);
        }
    }
}
