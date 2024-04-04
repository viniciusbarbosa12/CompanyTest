using Dao.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTO;
using Models.entities;
using Models.Utils;
using Newtonsoft.Json.Linq;
using Services.CategoryService;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService service;

        public CategoryController(ICategoryService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<Response>> Get()
        {
            try
            {
                var categories = await service.GetAll();

                if (!categories.IsOk)
                    return NotFound(categories);


                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }


        [HttpPost("getAllPaginated")]
        public async Task<ActionResult<Response>> Get(Pagination pagination)
        {
            try
            {
                var categories = await service.GetAllPaginated(pagination);

                if (!categories.IsOk)
                    return NotFound(categories);

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> GetById(Guid id)
        {
            try
            {
                var categoriy = await service.GetById(id);

                if (categoriy == null)
                    return NotFound();


                return Ok(categoriy);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }


        [HttpPost]
        public async Task<ActionResult<Response>> Create(CategoryDTO category)
        {
            try
            {
                if (category == null)
                {
                    return BadRequest("Category data is null.");
                }

                var result = await service.Create(category);

                return CreatedAtAction(nameof(GetById), new { result.Result }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Response>> Update(CategoryDTO categoryDto)
        {
            try
            {
                if (categoryDto.Id == Guid.Empty || categoryDto == null)
                {
                    return BadRequest("Invalid id or category data.");
                }

                var result = await service.Update(categoryDto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }

        }
    }
}
