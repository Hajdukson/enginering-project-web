using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Models
{
	public class BoughtProduct
	{
		public int Id { get; set; }
		[Required]
		public int ProductId { get; set; }
		[ValidateNever]
		public Product Product { get; set; }
		public int Quntity { get; set; }
		[Required]
		[Column(TypeName = "decimal(18,4)")]
		public decimal Price { get; set; }
		public DateTime BoughtDate { get; set; }
	}
}
