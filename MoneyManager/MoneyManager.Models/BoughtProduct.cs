using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyManager.Models
{
    public class BoughtProduct
	{
		public int Id { get; set; }
		[Required]
        public string Name { get; set; }
		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }
		public DateTime? BoughtDate { get; set; }
	}
}
