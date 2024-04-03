using Dao.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.entities;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            try
            {
                var products = await _context.Products.AsNoTracking().ToListAsync();

                if (products == null || products.Count == 0)
                    return NotFound();


                return Ok(products);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetById(Guid id)
        {
            try
            {
                var products = await _context.Products.AsNoTracking().FirstOrDefaultAsync(item => item.Id.Equals(id));

                if (products == null)
                    return NotFound();


                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }





        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest("Category data is null.");
                }

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Product product)
        {
            try
            {
                if (id == Guid.Empty || product == null)
                {
                    return BadRequest("Invalid id or category data.");
                }

                var existingProduct = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);

                if (existingProduct == null)
                {
                    return NotFound($"Category with id {id} not found.");
                }

                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;

                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();

                return Ok(existingProduct);


            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }
    }
}
