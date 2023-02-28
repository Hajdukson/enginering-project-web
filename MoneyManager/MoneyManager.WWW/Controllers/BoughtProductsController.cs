using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Models;
using MoneyManager.Repository;
using MoneyManager.Services.Interfeces;
using NuGet.Packaging.Rules;
using System.Diagnostics;
using System.Linq.Expressions;

namespace MoneyManager.WWW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoughtProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReceiptRecognizer _receiptRecognizer;
        private readonly IWebHostEnvironment _hostEnvironment;        
        public BoughtProductsController(IWebHostEnvironment hostEnvironment, IReceiptRecognizer receiptRecognizer, IUnitOfWork unitOfWork)
        {
            _hostEnvironment = hostEnvironment;
            _receiptRecognizer = receiptRecognizer;
            _unitOfWork = unitOfWork;
        }

        // GET: api/BoughtProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoughtProduct>>> GetBoughtProducts(string? name)
        {
            if (_unitOfWork.BoughtProduct == null)
            {
                return NotFound();
            }

            return await _unitOfWork.BoughtProduct.GetAll(new List<Expression<Func<BoughtProduct, bool>>> { 
                !string.IsNullOrEmpty(name) ? bp => bp.Name == name : null}).ToListAsync();
        }

        // GET: api/BoughtProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BoughtProduct>> GetBoughtProduct(int id)
        {
            if (_unitOfWork.BoughtProduct == null)
            {
                return NotFound();
            }
            var boughtProduct = await _unitOfWork.BoughtProduct
                .GetFirstOrDefaultAsync(b => b.Id == id);

            if (boughtProduct == null)
            {
                return NotFound();
            }

            return boughtProduct;
        }

        // PUT: api/BoughtProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBoughtProduct(int id, BoughtProduct boughtProduct)
        {
            if (id != boughtProduct.Id)
            {
                return BadRequest();
            }

            try
            {
                _unitOfWork.BoughtProduct.Update(boughtProduct);
                await _unitOfWork.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoughtProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpGet("Analize")]
        public async Task<ActionResult<IEnumerable<BoughtProduct>>> AnalizeImage(IFormFile? file)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if(file != null)
            {
                var filePath = Path.Combine(wwwRootPath, file.FileName);
                using (var fileStreams = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                };
                using (var fileStreams = new FileStream(filePath, 
                    FileMode.Open, 
                    FileAccess.Read, 
                    FileShare.None, 
                    4096, 
                    FileOptions.DeleteOnClose))
                {
                    try
                    {
                        return await _receiptRecognizer.AnalizeImage(fileStreams);
                    }
                    catch(BadHttpRequestException ex)
                    {
                        return BadRequest(new { error = "Error"});
                    }
                };
            }
            
            return Problem("Could not find any item");
        }
        [HttpPost("AddBoughtProducts")]
        public async Task<ActionResult<List<BoughtProduct>>> AddBoughtProducts([FromBody] IEnumerable<BoughtProduct> boughtProducts)
        {
            if(boughtProducts == null)
            {
                return Problem("'Enumerable<BoughtProduct> boughtProducts' was null.");
            }

            await _unitOfWork.BoughtProduct.AddRangeAsync(boughtProducts);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction("AddBoughtProducts", new {products = boughtProducts});
        }

        [HttpGet("DistinctProducts")]
        public async Task<ActionResult<IEnumerable<ProductSummary>>> GetDistinctProducts(string? name, DateTime? startDate, DateTime? endDate)
        {
            if(_unitOfWork.BoughtProduct == null)
            {
                return Problem("Entity set 'MoneyManagerWWWContext.BoughtProducts'  is null.");
            }            

            return await _unitOfWork.BoughtProduct.GetBoughtProductsSummaries(name, startDate, endDate);
        }

        // POST: api/BoughtProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BoughtProduct>> PostBoughtProduct(BoughtProduct boughtProduct)
        {
            await _unitOfWork.BoughtProduct.AddAsync(boughtProduct);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction("GetBoughtProduct", new { id = boughtProduct.Id }, boughtProduct);
        }

        // DELETE: api/BoughtProducts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BoughtProduct>> DeleteBoughtProduct(int id)
        {
            if (_unitOfWork.BoughtProduct == null)
            {
                return NotFound();
            }
            var boughtProduct = await _unitOfWork.BoughtProduct.GetFirstOrDefaultAsync(bp => bp.Id == id);
            if (boughtProduct == null)
            {
                return NotFound();
            }

            var removedProduct = _unitOfWork.BoughtProduct.Remove(entity: boughtProduct);
            await _unitOfWork.SaveAsync();

            return removedProduct;
        }
        [HttpDelete("DeleteByNames")]
        public async Task<ActionResult<List<BoughtProduct>>> DeleteProductsByName(IEnumerable<string> names)
        {
            if (_unitOfWork.BoughtProduct == null)
            {
                return NotFound();
            }

            var deletedProducts = _unitOfWork.BoughtProduct.DeleteProductsByName(names);

            if(deletedProducts.Count() == 0)
            {
                return NotFound("Cannnot delete products.");
            }

            await _unitOfWork.SaveAsync();

            return deletedProducts;
        }

        private bool BoughtProductExists(int id)
        {
            return _unitOfWork.BoughtProduct.GetAll().Any(e => e.Id == id);
        }
    }
}
