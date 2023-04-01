using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Models;
using MoneyManager.Repository;
using MoneyManager.Services.Interfeces;
using NuGet.Packaging.Rules;
using System;
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
        [HttpGet("ReceiptProducts")]
        public async Task<ActionResult<IEnumerable<BoughtProduct>>> GetProductsFromConcreteReceipt(string imagePath)
        {
            if (imagePath == null) 
            {
                return NotFound("Image not found");
            }
            return await _unitOfWork.BoughtProduct.GetProductsFromConcreteReceipt(imagePath).ToListAsync();
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

            return Ok();
        }
        [HttpDelete("DeleteImage")]
        public ActionResult DeleteImage(string imagePath)
        {
            var filePath = Path.Combine(_hostEnvironment.WebRootPath, @$"{imagePath}");
            if (imagePath == null) 
            {
                return NotFound("Path was null");
            }

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return Ok();
            }
            return NotFound($"Image not found 'path={imagePath}'");
        }
        [HttpGet("Analize")]
        public async Task<ActionResult<List<BoughtProduct>>> AnalizeImage(IFormFile? file)
        {
            if(file != null)
            {
                var fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(_hostEnvironment.WebRootPath, @"images\receipts");
                var fileExtension = Path.GetExtension(file.FileName);

                var image = Path.Combine(uploads, fileName + fileExtension);

                using (var fileStreams = new FileStream(image, FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                };

                try
                {
                    using (var fileStreams = new FileStream(image,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.None,
                    4096))
                    {
                        var products = await _receiptRecognizer.AnalizeImage(fileStreams);
                        products.ForEach(p => p.ImagePath = @"images\receipts\" + fileName + fileExtension);
                        return products;
                    }
                }
                catch (BadHttpRequestException ex)
                {
                    return BadRequest(new { error = ex.Message });
                }
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

        [HttpGet("download")]
        public IActionResult Download()
        {
            var filepath = Path.Combine(_hostEnvironment.ContentRootPath, "ReceiptImages", "image.jpg");
            return File(System.IO.File.ReadAllBytes(filepath), "image/jpg", System.IO.Path.GetFileName(filepath));
        }

        private bool BoughtProductExists(int id)
        {
            return _unitOfWork.BoughtProduct.GetAll().Any(e => e.Id == id);
        }
    }
}
