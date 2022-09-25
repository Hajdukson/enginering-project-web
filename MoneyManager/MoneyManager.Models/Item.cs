
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyManager.Models
{
    public class Item : IItem
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get ; set; }
        [Required]
        public virtual ItemType Type { get ; }
    }
}
