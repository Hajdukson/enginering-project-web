using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Models;
using MoneyManager.Models.DTOs;
using MoneyManager.Repository;
using MoneyManager.Services.Interfeces;
using MoneyManager.Utility;
using System.Data;
using System.Linq.Expressions;
using System.Security.Claims;

namespace MoneyManager.WWW.Controllers
{
    // FUTURE UPDATES, TODOS
    // Whole Controller requiers refactor in order to work as assumed
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : Controller
    {
        #region CTOR, PROPS
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
        public async Task<ActionResult<UserPanelDTO>> GetAllItems(
            string? searchString, 
            ItemType? type, 
            DateTime? startDate, 
            DateTime? endDate)
        {
            try
            {
                UserPanelDTO userPanelDTO = new UserPanelDTO();

                var sortedItems = _dbContext.Items.GetAll().ToList();

                if (type != null)
                    sortedItems = sortedItems.Where(i => i.Type == type).ToList();

                var itemsDTO = sortedItems.Select(item => item.ConverToItemsDTO()).ToList();
                var loggedUser = await _dbContext.ApplicationUsers.GetFirstOrDefaultAsync(u => u.Id == LoggedUserId);

                userPanelDTO.Items = itemsDTO;
                userPanelDTO.TotalIncome = _expenseCalculator.CalculateIncome(sortedItems);
                userPanelDTO.TotalOutcome = _expenseCalculator.CalculateOutcome(sortedItems);
                userPanelDTO.Balance = _expenseCalculator.CalculateBalance(sortedItems);
                userPanelDTO.AppUser = loggedUser.ConverToUserDTO();

                return userPanelDTO;
            }
            catch (DataException ex)
            {
                Console.WriteLine(ex.StackTrace);
                return NotFound(new { message = "Could not find any object." });
            }
        }
        #endregion

        #region GET ITEM DETAILS
        [HttpGet]
        [Route("details")]
        public async Task<ActionResult<ItemDetailsDTO>> Details(int? id)
        {
            if (_dbContext.Items == null || _dbContext.Items.GetAll().Count() == 0)
            {
                return NotFound(new { message = "Can not find any object." });
            }

            var item = await _dbContext.Items.GetFirstOrDefaultAsync(i => i.Id == id, "IncomeCategory,OutcomeCategory");

            if(item == null)
            {
                return NotFound(new { message = $"{item} not found." });
            }

            return item.ConverToItemDetailsDTO();
        }
        #endregion

        #region ADD ITEMS
        [HttpPost("addincome")]
        public async Task<ActionResult<Income>> AddIncome([FromBody] Income income)
        {
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

            return CreatedAtAction(nameof(AddIncome), new {message = "object created successfully" });
        }

        [HttpPost("addoutcome")]
        public async Task<ActionResult<Outcome>> AddOutcome([FromBody] Outcome outcome)
        {
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

            return CreatedAtAction(nameof(AddOutcome), new { message = "object created successfully" });
        }
        #endregion

        #region EDIT ITEMS
        [HttpPut("editincome/{id}")]
        public async Task<IActionResult> PutIncome(int id, Income income)
        {
            if (id == 0)
            {
                return NotFound();
            }

            income.ApplicationUserId = LoggedUserId;

            if (ModelState.IsValid)
            {
                _dbContext.Incomes.Update(income);
                await _dbContext.SaveAsync();
                return Ok(new { message = "object edited successfully" });
            }

            return BadRequest();
        }

        [HttpPut("editoutcome/{id}")]
        public async Task<IActionResult> PutOutcome(int id, Outcome outcome)
        {
            if(id == 0)
            {
                return NotFound();
            }

            outcome.ApplicationUserId = LoggedUserId;

            if (ModelState.IsValid)
            {
                _dbContext.Outcomes.Update(outcome);
                await _dbContext.SaveAsync();
                return Ok(new { message = "object edited successfully" });
            }

            return BadRequest();
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