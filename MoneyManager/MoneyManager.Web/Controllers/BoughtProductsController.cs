using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoneyManager.DataAccess;
using MoneyManager.Models;

namespace MoneyManager.Web.Controllers
{
    public class BoughtProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BoughtProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult DisplyAvaliableProducts()
        {
            return View();
        }
        // GET: BoughtProducts
        public async Task<IActionResult> Index()
        {
            return _unitOfWork.BoughtProduct != null ?
                        View(await _unitOfWork.BoughtProduct.GetAllAsync("Product.Category")) :
                        Problem("Entity set 'MoneyManagerDataContext.BoughtProduct'  is null.");
        }
        public async Task<IActionResult> AddProduct(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            var product = await _unitOfWork.Product.GetFirstOrDefaultAsync(p => p.Id == id, includeProperties: "Category");

            if(product == null)
            {
                return NotFound();
            }

            var boughtProduct = new BoughtProduct()
            {
                ProductId = id,
                Product = product,
                Price = 0,
                Quntity = 1,
            };

            return View(boughtProduct);
        }

        // POST: BoughtProducts/Upsert
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(BoughtProduct boughtProduct)
        {
            if (ModelState.IsValid)
            {
                if (boughtProduct.Id == 0)
                {
                    try
                    {
                        await _unitOfWork.BoughtProduct.AddAsync(boughtProduct);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BoughtProductExists(boughtProduct.Id))
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
                    return NotFound();
                }
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(DisplyAvaliableProducts));
            }
            return View(nameof(Index));
        }

        // POST: BoughtProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_unitOfWork.BoughtProduct == null)
            {
                return Problem("Entity set 'MoneyManagerDataContext.BoughtProduct'  is null.");
            }
            var boughtProduct = await _unitOfWork.BoughtProduct.GetFirstOrDefaultAsync(c => c.Id == id);
            if (boughtProduct != null)
            {
                _unitOfWork.BoughtProduct.Remove(boughtProduct);
            }

            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoughtProductExists(int id)
        {
            return (_unitOfWork.BoughtProduct?.GetAllAsync().Result.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
