﻿using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Repository
{
    public interface IIncomeRepository : IRepository<Income>
    {
        void Update(Income item);
    }
}
