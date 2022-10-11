using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Models.DTOs
{
    public class OutcomeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public OutcomeCategory IncomeCategory { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
