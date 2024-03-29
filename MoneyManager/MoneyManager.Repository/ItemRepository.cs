﻿using MoneyManager.Models;
using MoneyManager.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Repository
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        private readonly MoneyManagerContext _dbContext;
        public ItemRepository(MoneyManagerContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(Item item)
        {
            _dbContext.Items.Update(item);
        }
    }
}
