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
        decimal CalculateBalance(List<IItem> items);
        decimal CalculateIncome(List<IItem> items);
        decimal CalculateOutcome(List<IItem> items);
    }
}
