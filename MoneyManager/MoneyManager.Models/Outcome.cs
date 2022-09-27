using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MoneyManager.Models;

namespace MoneyManager.Models
{
    public class Outcome : Item
    {
        [ValidateNever]
        public OutcomeCategory OutcomeCategory { get; set; }
        public int OutcomeCategoryId { get; set; }
        public override ItemType Type => ItemType.Outcome;
    }
}
