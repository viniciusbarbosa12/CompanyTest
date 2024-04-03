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
            var categories = await _context.Categories.ToListAsync();

            if (categories == null || categories.Count == 0)
                return NotFound();


            return Ok(categories);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetById(Guid id)
        {
            var categories = await _context.Categories.FirstOrDefaultAsync(item => item.Id.Equals(id));

            if (categories == null)
                return NotFound();


            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Create(Category category)
        {
            if (category == null)
            {
                return BadRequest("Category data is null.");
            }

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Category category)
        {
            if (id == Guid.Empty || category == null)
            {
                return BadRequest("Invalid id or category data.");
            }

            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (existingCategory == null)
            {
                return NotFound($"Category with id {id} not found.");
            }

            existingCategory.Name = category.Name;
            existingCategory.Codigo = category.Codigo;

            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();

            return Ok(existingCategory);
        }
    }
}
