using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Models;
using MoneyManager.WWW.Data;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace MoneyManager.WWW.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : Controller
    {
        private readonly MoneyManagerContext _context;
        private readonly List<Item> _items;

        public ItemsController(MoneyManagerContext context)
        {
            _context = context;
            _items = new List<Item>();
        }
        private List<Item> GetItemsList()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var items = new List<Item>();
            items.AddRange(_context.Incomes.Include(c => c.IncomeCategory));
            items.AddRange(_context.Outcomes.Include(c => c.OutcomeCategory));
            items = items.Where(u => u.ApplicatioUserId == claim.Value).ToList();

            return items;
        }
        // GET: api/Items
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<Item>> Index()
        {
            var itmes = GetItemsList();
            return itmes;
        }

        // GET: api/Items/Details/
        [HttpGet]
        [Route("details")]
        public ActionResult<Item> Details(int id, ItemType itemType)
        {
            var itmes = GetItemsList();
            var item = itmes.First(i => i.Id == id && i.Type == itemType);
            return item;
        }
        // GET: api/Items/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST api/Items/Add-Income
        [HttpPost("addincome")]
        public async Task<ActionResult<Income>> AddIncome([FromBody] Income income)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            income.ApplicatioUserId = claim.Value;

            await _context.Incomes.AddAsync(income);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(AddIncome), income);
        }
        // POST api/Items/Add-Outcome
        [HttpPost("addoutcome")]
        public async Task<ActionResult<Outcome>> AddOutcome([FromBody] Outcome outcome)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            outcome.ApplicatioUserId = claim.Value;

            await _context.Outcomes.AddAsync(outcome);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(AddOutcome), outcome);
        }

        // POST: api/Items/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: api/Items/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: api/Items/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
