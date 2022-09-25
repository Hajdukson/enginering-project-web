using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Web.WebPages.Html;

namespace MoneyManager.Models
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
