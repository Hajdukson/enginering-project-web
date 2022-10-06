
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyManager.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get ; set; } 
        [NotMapped]
        public virtual ItemType Type { get ; }
        public string IdentityUserId { get; set; }
        [ValidateNever]
        [ForeignKey("IdentityUserId")]
        public IdentityUser IdentityUser { get; set; }
    }

    public enum ItemType
    {
        Income,
        Outcome
    }
}
