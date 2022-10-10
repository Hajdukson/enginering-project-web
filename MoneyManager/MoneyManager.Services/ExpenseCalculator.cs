using MoneyManager.Models;
using MoneyManager.Services.Interfeces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Services
{
    public class ExpenseCalculator : ICalculator
    {
        public decimal CalculateBalance(IEnumerable<Item> items) =>
            CalculateIncome(items) - CalculateOutcome(items);

        public decimal CalculateIncome(IEnumerable<Item> items) =>
            items.Aggregate(0.0m, (total, item) => item.Type == ItemType.Income ? total + item.Price : total);

        public decimal CalculateOutcome(IEnumerable<Item> items) =>
            items.Aggregate(0.0m, (total, item) => item.Type == ItemType.Outcome ? total + item.Price : total);
    }
}
