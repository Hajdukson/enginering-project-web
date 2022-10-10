using MoneyManager.Models;
using MoneyManager.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Repository
{
    public class OutcomeRepository : Repository<Outcome>, IOutcomeRepository
    {
        private readonly MoneyManagerContext _dbContext;
        public OutcomeRepository(MoneyManagerContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(Outcome item)
        {
            _dbContext.Outcomes.Update(item);
        }
    }
}
