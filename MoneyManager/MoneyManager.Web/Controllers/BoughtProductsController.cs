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
        // GET: BoughtProducts
        public async Task<IActionResult> Index()
        {
            return _unitOfWork.BoughtProduct != null ?
                        View(await _unitOfWork.BoughtProduct.GetAllAsync()) :
                        Problem("Entity set 'MoneyManagerDataContext.BoughtProduct'  is null.");
        }
        // GET: BoughtProducts/Upsert
        public async Task<IActionResult> Upsert(int? id)
        {
            var boughtProductViewModel = new BoughtProductViewModel() 
            {
                Categories = _unitOfWork.Category.GetAll().Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }),
                Products = _unitOfWork.Product.GetAll().Select(p => new SelectListItem() 
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                })
            };

            if (id == 0 || id == null)
            {
                return View(boughtProductViewModel);
            }

            boughtProductViewModel.BoughtProduct = await _unitOfWork.BoughtProduct.GetFirstOrDefaultAsync(c => c.Id == id);
            return View(boughtProductViewModel);
        }

        // POST: BoughtProducts/Upsert
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(BoughtProductViewModel boughtProductViewModel)
        {
            if (ModelState.IsValid)
            {
                if (boughtProductViewModel.BoughtProduct.Id != 0)
                {
                    try
                    {
                        _unitOfWork.BoughtProduct.Update(boughtProductViewModel.BoughtProduct);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BoughtProductExists(boughtProductViewModel.BoughtProduct.Id))
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
                    await _unitOfWork.BoughtProduct.AddAsync(boughtProductViewModel.BoughtProduct);
                }
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(boughtProductViewModel.BoughtProduct);
        }

        // GET: BoughtProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _unitOfWork.BoughtProduct == null)
            {
                return NotFound();
            }

            var boughtProduct = await _unitOfWork.BoughtProduct
                .GetFirstOrDefaultAsync(m => m.Id == id);
            if (boughtProduct == null)
            {
                return NotFound();
            }

            return View(boughtProduct);
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
