using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Models
{
    public class ProductSummary
    {
        public string Name { get; set; }
        public BoughtProduct StartProduct { get; set; }
        public BoughtProduct EndProduct { get; set; }
        public decimal Inflation { get; set; }
    }
}
