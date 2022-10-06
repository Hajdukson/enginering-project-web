using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MoneyManager.Models;
using MoneyManager.Models.DTOs;
using MoneyManager.Services.Interfeces;
using MoneyManager.WWW.Data;
using NuGet.Protocol.Core.Types;
using System.Data;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace MoneyManager.WWW.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : Controller
    {
        private readonly ICalculator _expenseCalculator;
        private readonly MoneyManagerContext _context;
        private readonly List<Item> _items;

        public ItemsController(MoneyManagerContext context, ICalculator expenseCalculator)
        {
            _context = context;
            _items = new List<Item>();
            _expenseCalculator = expenseCalculator;
        }
        private async Task<List<Item>> GetItemsList()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var items = new List<Item>();

            var incomes = await _context.Incomes
                .Include(i => i.IncomeCategory)
                .Include(u => u.IdentityUser).ToListAsync();

            var outcomes = await _context.Outcomes
                .Include(o => o.OutcomeCategory)
                .Include(u => u.IdentityUser).ToListAsync();

            items.AddRange(incomes);
            items.AddRange(outcomes);

            items = items.Where(i => i.IdentityUserId == claim.Value).ToList();

            return items;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ItemsDTO>> Index()
        {
            ItemsDTO itemsDTO = new ItemsDTO();
            try
            {
                var items = await GetItemsList();

                itemsDTO.Items = items;
                itemsDTO.TotalIncome = _expenseCalculator.CalculateIncome(items);
                itemsDTO.TotalOutcome = _expenseCalculator.CalculateOutcome(items);
                itemsDTO.Balance = _expenseCalculator.CalculateBalance(items);

                return itemsDTO;
            }
            catch (DataException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("details")]
        public async Task<ActionResult<Item>> Details(int? id)
        {
            var itmes = await GetItemsList();
            var item = itmes.Find(i => i.Id == id);

            if (item != null)
            {
                return item;
            }

            return NotFound(new { message = $"item with '{id}' not found" });
        }

        [HttpPost("addincome")]
        public async Task<ActionResult<Income>> AddIncome([FromBody] Income income)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return NotFound(new { message = "app user not found" });
            }

            var incomeCategory = await _context.IncomeCategories.FindAsync(income.IncomeCategoryId);

            if (incomeCategory == null)
            {
                return NotFound(new { message = "incomeCategory not found" });
            }

            if (income == null || income.Id != 0)
            {
                return BadRequest(new { message = "income was null" });
            }

            income.IdentityUserId = claim.Value;

            await _context.Incomes.AddAsync(income);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(AddIncome), income);
        }

        [HttpPost("addoutcome")]
        public async Task<ActionResult<Outcome>> AddOutcome([FromBody] Outcome outcome)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return NotFound(new { message = "app user not found" });
            }

            var outcomeCategory = await _context.OutcomeCategories.FindAsync(outcome.OutcomeCategoryId);

            if (outcomeCategory == null)
            {
                return NotFound(new { message = "outcomeCategory not found" });
            }

            if (outcome == null || outcome.Id != 0)
            {
                return BadRequest(new { message = "outcome was null" });
            }

            outcome.IdentityUserId = claim.Value;

            await _context.Outcomes.AddAsync(outcome);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(AddOutcome), outcome);
        }

        
        [HttpPut("editincome/{id}")]
        public async Task<IActionResult> PutIncome(int id, [FromBody] Income item)
        {
            var itemFromDb = await _context.Incomes.FindAsync(id);

            if (itemFromDb == null)
            {
                return NotFound(new { message = $"item witd id:{id} not found" });
            }

            itemFromDb.Price = item.Price;
            itemFromDb.Name = item.Name;
            itemFromDb.TransactionDate = item.TransactionDate;
            itemFromDb.IncomeCategory = item.IncomeCategory;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editoutcome/{id}")]
        public async Task<IActionResult> PutOutcome(int id, [FromBody] Outcome item)
        {
            var itemFromDb = await _context.Outcomes.FindAsync(id);

            if (itemFromDb == null)
            {
                return NotFound(new { message = $"item with id:{id} not found" });
            }

            itemFromDb.Price = item.Price;
            itemFromDb.Name = item.Name;
            itemFromDb.TransactionDate = item.TransactionDate;
            itemFromDb.OutcomeCategory = item.OutcomeCategory;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if(item == null)
            {
                return NotFound(new { message = $"item with id: {id} not found" });
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
