using Dao.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTO;
using Models.entities;
using Models.Utils;
using Services.ProductService;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService service;
        public ProductController(IProductService service)
        {
            this.service = service;
        }


        [HttpGet]
        public async Task<ActionResult<Response>> Get()
        {
            try
            {
                var products = await service.GetAll();

                if (!products.IsOk)
                    return NotFound(products);


                return Ok(products);
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
                var products = await service.GetAllPaginated(pagination);

                if (!products.IsOk)
                    return NotFound(products);

                return Ok(products.Result);
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
                var product = await service.GetById(id);

                if (product == null)
                    return NotFound();


                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }


        [HttpPost]
        public async Task<ActionResult<Response>> Create(ProductDTO product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest("Product params is null");
                }

                var result = await service.Create(product);

                return result.IsOk ? StatusCode(201, result) : BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Response>> Update(ProductDTO product)
        {
            try
            {
                if (product.Id == Guid.Empty || product == null)
                {
                    return BadRequest("Invalid id or category data.");
                }

                var result = await service.Update(product);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }

        }
    }
}
