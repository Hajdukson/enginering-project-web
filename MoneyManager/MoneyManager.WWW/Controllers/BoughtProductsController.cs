using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Models;
using MoneyManager.Repository;
using MoneyManager.Services.Interfeces;

namespace MoneyManager.WWW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoughtProductsController : ControllerBase
    {
        private readonly MoneyManagerContext _context;
        private readonly IReceiptRecognizer _receiptRecognizer;
        private readonly IWebHostEnvironment _hostEnvironment;
        public BoughtProductsController(MoneyManagerContext context, IWebHostEnvironment hostEnvironment, IReceiptRecognizer receiptRecognizer)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _receiptRecognizer = receiptRecognizer;
        }

        // GET: api/BoughtProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoughtProduct>>> GetBoughtProducts()
        {
            if (_context.BoughtProducts == null)
            {
                return NotFound();
            }

            var products = await _context.BoughtProducts
                .ToListAsync();

            return products;
        }

        // GET: api/BoughtProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BoughtProduct>> GetBoughtProduct(int id)
        {
            if (_context.BoughtProducts == null)
            {
                return NotFound();
            }
            var boughtProduct = await _context.BoughtProducts
                .FirstAsync(b => b.Id == id);

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

            _context.Entry(boughtProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
        [HttpGet("analize")]
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
                        throw;
                    }
                    
                };
            }
            
            return Problem("Could not find any item");
        }
        [HttpPost("addboughtproducts")]
        public async Task<ActionResult<List<BoughtProduct>>> AddBoughtProducts([FromBody] IEnumerable<BoughtProduct> boughtProducts)
        {
            if(boughtProducts == null)
            {
                return Problem("'Enumerable<BoughtProduct> boughtProducts' was null or empty.");
            }

            await _context.BoughtProducts.AddRangeAsync(boughtProducts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("AddBoughtProducts", new {products = boughtProducts});
        }

        // POST: api/BoughtProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BoughtProduct>> PostBoughtProduct(BoughtProduct boughtProduct)
        {
            if (_context.BoughtProducts == null)
            {
                return Problem("Entity set 'MoneyManagerWWWContext.BoughtProducts'  is null.");
            }
            _context.BoughtProducts.Add(boughtProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBoughtProduct", new { id = boughtProduct.Id }, boughtProduct);
        }

        // DELETE: api/BoughtProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoughtProduct(int id)
        {
            if (_context.BoughtProducts == null)
            {
                return NotFound();
            }
            var boughtProduct = await _context.BoughtProducts.FindAsync(id);
            if (boughtProduct == null)
            {
                return NotFound();
            }

            _context.BoughtProducts.Remove(boughtProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BoughtProductExists(int id)
        {
            return (_context.BoughtProducts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
