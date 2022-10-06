using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Models.DTOs
{
    public class ItemsDTO
    {
        public List<Item>? Items { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalOutcome { get; set; }
    }
}
