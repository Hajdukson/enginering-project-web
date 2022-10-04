using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoneyManager.Models;
using MoneyManager.WWW.Data;

namespace MoneyManager.WWW.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly MoneyManager.WWW.Data.MoneyManagerContext _context;

        public CreateModel(MoneyManager.WWW.Data.MoneyManagerContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Products == null || Product == null)
            {
                return Page();
            }
            var category = await _context.ProductCategories.FindAsync(Product.ProductCategoryId);

            Product.ProductCategory = category;

            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
