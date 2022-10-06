﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Models
{
    public class Income : Item
    {
        [Required]
        [ValidateNever]
        public IncomeCategory IncomeCategory { get; set; }
        public int IncomeCategoryId { get; set; }
        public override ItemType Type => ItemType.Income;
    }
}
