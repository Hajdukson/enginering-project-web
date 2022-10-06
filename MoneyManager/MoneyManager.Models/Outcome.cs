using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Models
{
    public class Outcome : Item
    {
        [Required]
        [ValidateNever]
        public OutcomeCategory OutcomeCategory { get; set; }
        public int OutcomeCategoryId { get; set; }
        public override ItemType Type => ItemType.Outcome;
    }
}
