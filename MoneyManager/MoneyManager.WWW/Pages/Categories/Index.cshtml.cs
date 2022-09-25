using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Models;
using MoneyManager.WWW.Data;

namespace MoneyManager.WWW.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly MoneyManager.WWW.Data.MoneyManagerWWWContext _context;

        public IndexModel(MoneyManager.WWW.Data.MoneyManagerWWWContext context)
        {
            _context = context;
        }

        public IList<Category> Categoires { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Categories != null)
            {
                Categoires = await _context.Categories.ToListAsync();
            }
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
