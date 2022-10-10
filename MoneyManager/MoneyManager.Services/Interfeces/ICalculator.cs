using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Models;

namespace MoneyManager.Services.Interfeces
{
    public interface ICalculator
    {
        decimal CalculateBalance(IEnumerable<Item> items);
        decimal CalculateIncome(IEnumerable<Item> items);
        decimal CalculateOutcome(IEnumerable<Item> items);
    }
}
