using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyManager.DataAccess;
using MoneyManager.Models;

namespace MoneyManager.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
              return _unitOfWork.Category != null ? 
                          View(await _unitOfWork.Category.GetAllAsync()) :
                          Problem("Entity set 'MoneyManagerWWWContext.Category'  is null.");
        }
        // GET: Categories/Upsert
        public async Task<IActionResult> Upsert(int? id)
        {
            var editCategory = new Category();
            if (id == 0 || id == null)
            {
                return View(editCategory);
            }

            editCategory = await _unitOfWork.Category.GetFirstOrDefaultAsync(c => c.Id == id);
            return View(editCategory);
        }

        // POST: Categories/Upsert
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert([Bind("Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                if(category.Id != 0)
                {
                    try
                    {
                        _unitOfWork.Category.Update(category);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CategoryExists(category.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    await _unitOfWork.Category.AddAsync(category);
                }
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_unitOfWork.Category == null)
            {
                return Problem("Entity set 'MoneyManagerWWWContext.Category'  is null.");
            }
            var category = await _unitOfWork.Category.GetFirstOrDefaultAsync(c => c.Id == id);
            if (category != null)
            {
                _unitOfWork.Category.Remove(category);
            }
            
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_unitOfWork.Category?.GetAllAsync().Result.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
