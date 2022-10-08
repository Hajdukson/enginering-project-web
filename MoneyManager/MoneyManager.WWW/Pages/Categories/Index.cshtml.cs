using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Models;
using MoneyManager.Repository;

namespace MoneyManager.WWW.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly MoneyManagerContext _context;

        public IndexModel(MoneyManagerContext context)
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
    }
}
