﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Models;
using MoneyManager.Repository;

namespace MoneyManager.WWW.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly MoneyManagerContext _context;

        public IndexModel(MoneyManagerContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {
                Product = await _context.Products
                .Include(p => p.ProductCategory).ToListAsync();
            }
        }
    }
}
