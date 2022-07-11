using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MoneyManager.WWW.Data;
using MoneyManager.WWW.Models;

namespace MoneyManager.WWW.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly MoneyManager.WWW.Data.MoneyManagerWWWContext _context;

        public IndexModel(MoneyManager.WWW.Data.MoneyManagerWWWContext context)
        {
            _context = context;
        }

        public IList<Category> Category { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Category != null)
            {
                Category = await _context.Category.ToListAsync();
            }
        }
    }
}
