using Dao.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.entities;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            try
            {
                var categories = await _context.Categories.AsNoTracking().ToListAsync();

                if (categories == null || categories.Count == 0)
                    return NotFound();


                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetById(Guid id)
        {
            try
            {
                var categories = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(item => item.Id.Equals(id));

                if (categories == null)
                    return NotFound();


                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }


        [HttpGet("products/{id}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesWithProducts(Guid id)
        {
            try
            {
                var categories = _context.Categories.AsNoTracking().Include(p => p.Products).Where(item => item.Id.Equals(id)).ToListAsync();

                if (categories == null)
                    return NotFound();


                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Create(Category category)
        {
            try
            {
                if (category == null)
                {
                    return BadRequest("Category data is null.");
                }

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Category category)
        {
            try
            {
                if (id == Guid.Empty || category == null)
                {
                    return BadRequest("Invalid id or category data.");
                }

                var existingCategory = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

                if (existingCategory == null)
                {
                    return NotFound($"Category with id {id} not found.");
                }

                existingCategory.Name = category.Name;
                existingCategory.Code = category.Code;

                _context.Categories.Update(existingCategory);
                await _context.SaveChangesAsync();

                return Ok(existingCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }

        }
    }
}
