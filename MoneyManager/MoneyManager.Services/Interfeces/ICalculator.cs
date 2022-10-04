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
        decimal CalculateBalance(List<Item> items);
        decimal CalculateIncome(List<Item> items);
        decimal CalculateOutcome(List<Item> items);
    }
}
