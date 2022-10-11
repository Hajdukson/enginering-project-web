using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Models;
using MoneyManager.Models.DTOs;
using MoneyManager.Repository;
using MoneyManager.Services.Interfeces;
using MoneyManager.Utility;
using System.Data;
using System.Security.Claims;

namespace MoneyManager.WWW.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : Controller
    {
        #region CTOR, PRIVETE FIELDS
        private readonly IUnitOfWork _dbContext;
        private readonly ICalculator _expenseCalculator;
        private readonly IClaimUserId _claimeUser;
        private string LoggedUserId { get => _claimeUser.GetUserId((ClaimsIdentity)User.Identity); }
        public ItemsController(
            IUnitOfWork context, 
            ICalculator expenseCalculator, 
            IClaimUserId clameUser)
        {
            _dbContext = context;
            _expenseCalculator = expenseCalculator;
            _claimeUser = clameUser;
        }
        #endregion

        #region GET ALL ITEMS
        [HttpGet]
        public async Task<ActionResult<UserPanelDTO>> Index()
        {
            try
            {
                UserPanelDTO userPanelDTO = new UserPanelDTO();
                var items = await _dbContext.Items.GetAllAsync(nameof(ApplicationUser));
                var itemDTOs = items.Select(item => item.ConverToItemDTO()).ToList();
                var loggedUser = await _dbContext.ApplicationUsers.GetFirstOrDefaultAsync(u => u.Id == LoggedUserId);

                userPanelDTO.Items = itemDTOs;
                userPanelDTO.TotalIncome = _expenseCalculator.CalculateIncome(items);
                userPanelDTO.TotalOutcome = _expenseCalculator.CalculateOutcome(items);
                userPanelDTO.Balance = _expenseCalculator.CalculateBalance(items);
                userPanelDTO.AppUser = loggedUser.ConverToUserDTO();

                return userPanelDTO;
            }
            catch (DataException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            return BadRequest();
        }
        #endregion

        #region GET ITEM DETAILS
        [HttpGet]
        [Route("details")]
        public async Task<ActionResult<ItemDetailsDTO>> Details(int? id)
        {
            if (_dbContext.Items == null || _dbContext.Items.GetAll().Count() == 0)
            {
                return NotFound(new { message = "There are no objects in Database." });
            }

            var item = await _dbContext.Items.GetFirstOrDefaultAsync(i => i.Id == id, "IncomeCategory,OutcomeCategory");

            return item.ConverToItemDetailsDTO();
        }
        #endregion

        #region ADD ITEMS
        [HttpPost("addincome")]
        public async Task<ActionResult<Income>> AddIncome([FromBody] Income income)
        {
            if (LoggedUserId == null)
            {
                return NotFound(new { message = "app user not found" });
            }

            var incomeCategory = await _dbContext.Categories.GetFirstOrDefaultAsync(c => c.Id == income.IncomeCategoryId);

            if (incomeCategory == null)
            {
                return NotFound(new { message = "incomeCategory not found" });
            }

            if (income == null || income.Id != 0)
            {
                return BadRequest(new { message = "income was null" });
            }

            income.ApplicationUserId = LoggedUserId;

            await _dbContext.Incomes.AddAsync(income);
            await _dbContext.SaveAsync();

            return CreatedAtAction(nameof(AddIncome), income);
        }

        [HttpPost("addoutcome")]
        public async Task<ActionResult<Outcome>> AddOutcome([FromBody] Outcome outcome)
        {
            if (LoggedUserId == null)
            {
                return NotFound(new { message = "app user not found" });
            }

            var outcomeCategory = await _dbContext.Categories.GetFirstOrDefaultAsync(c => c.Id == outcome.OutcomeCategoryId);

            if (outcomeCategory == null)
            {
                return NotFound(new { message = "outcomeCategory not found" });
            }

            if (outcome == null || outcome.Id != 0)
            {
                return BadRequest(new { message = "outcome was null" });
            }

            outcome.ApplicationUserId = LoggedUserId;

            await _dbContext.Outcomes.AddAsync(outcome);
            await _dbContext.SaveAsync();

            return CreatedAtAction(nameof(AddOutcome), outcome);
        }
        #endregion

        #region EDIT ITEMS
        [HttpPut("editincome/{id}")]
        public async Task<IActionResult> PutIncome(int id, Income item)
        {
            //var itemFromDb = await _dbContext.Incomes.GetFirstOrDefaultAsync(i => i.Id == id);
            if (id == 0)
            {
                return NotFound();
            }

            //if (itemFromDb == null)
            //{
            //    return NotFound(new { message = $"item witd id:{id} not found" });
            //}

            //itemFromDb.Price = item.Price;
            //itemFromDb.Name = item.Name;
            //itemFromDb.TransactionDate = item.TransactionDate;
            //itemFromDb.IncomeCategory = item.IncomeCategory;
            _dbContext.Incomes.Update(item);
            await _dbContext.SaveAsync();

            return Ok();
        }

        [HttpPut("editoutcome/{id}")]
        public async Task<IActionResult> PutOutcome(int id, Outcome item)
        {
            //var itemFromDb = await _dbContext.Outcomes.GetFirstOrDefaultAsync(o => o.Id == id);
            if(id == 0)
            {
                return NotFound();
            }
            //if (itemFromDb == null)
            //{
            //    return NotFound(new { message = $"item with id:{id} not found" });
            //}

            _dbContext.Outcomes.Update(item);
            await _dbContext.SaveAsync();

            return Ok();
        }
        #endregion

        #region DELETE ITEM
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _dbContext.Items.GetFirstOrDefaultAsync(i => i.Id == id);

            if(item == null)
            {
                return NotFound(new { message = $"item with id: {id} not found" });
            }

            _dbContext.Items.Remove(item);
            await _dbContext.SaveAsync();

            return Ok();
        }
        #endregion
    }
}