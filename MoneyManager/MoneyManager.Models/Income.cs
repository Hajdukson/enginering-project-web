using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MoneyManager.Models
{
    public class Income : Item
    {
        [ValidateNever]
        public IncomeCategory IncomeCategory { get; set; }
        public int IncomeCategoryId { get; set; }
        public override ItemType Type => ItemType.Income;
    }
}
