using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoneyManager.DataAccess;
using MoneyManager.Models;

namespace MoneyManager.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #region ACTIONS
        // GET: Products
        public async Task<IActionResult> Index()
        {
            return _unitOfWork.Product != null ?
                        View(await _unitOfWork.Product.GetAllAsync("Category")) :
                        Problem("Entity set 'MoneyManagerDataContext.Product'  is null.");
        }
        // GET: Products/Upsert
        public async Task<IActionResult> Upsert(int? id)
        {
            var productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                Categories = _unitOfWork.Category.GetAll().Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
            };
            if (id == 0 || id == null)
            {
                return View(productViewModel);
            }

            productViewModel.Product = await _unitOfWork.Product.GetFirstOrDefaultAsync(c => c.Id == id, "Category");
            return View(productViewModel);
        }

        // POST: Products/Upsert
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductViewModel productVM)
        {
            if (ModelState.IsValid)
            {
                if (productVM.Product.Id != 0)
                {
                    try
                    {
                        _unitOfWork.Product.Update(productVM.Product);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductExists(productVM.Product.Id))
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
                    await _unitOfWork.Product.AddAsync(productVM.Product);
                }
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productVM);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_unitOfWork.Product == null)
            {
                return Problem("Entity set 'MoneyManagerDataContext.Product'  is null.");
            }
            var product = await _unitOfWork.Product.GetFirstOrDefaultAsync(c => c.Id == id);
            if (product != null)
            {
                _unitOfWork.Product.Remove(product);
            }

            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_unitOfWork.Product?.GetAllAsync().Result.Any(e => e.Id == id)).GetValueOrDefault();
        }
        #endregion
        #region API CALLS
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return Json(new { data = products });
        }
        #endregion
    }
}
