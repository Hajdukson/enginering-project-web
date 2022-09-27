using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Models;
using MoneyManager.WWW.Data;

namespace MoneyManager.WWW.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : Controller
    {
        private readonly MoneyManagerWWWContext _context;

        public ItemsController(MoneyManagerWWWContext context)
        {
            _context = context;
        }

        // GET: api/Items
        [HttpGet]
        public ActionResult Index()
        {
            return Ok("Items here");
        }

        // GET: api/Items/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: api/Items/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST api/Items/AddIncome
        public ActionResult AddIncome(Income income)
        {
            return View();
        }
        // POST api/Items/AddOutcome
        public ActionResult AddOutcome(Outcome outcome)
        {
            return View();
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
