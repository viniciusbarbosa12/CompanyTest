using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Models.Utils;
using Services.ProductService;

namespace API.Controllers
{
    [Authorize]
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
                var result = await service.GetAll();

                if (!result.IsOk)
                    return NotFound();


                return result.IsOk ? Ok(result) : BadRequest(result.Message);
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
                var result = await service.GetAllPaginated(pagination);

                if (!result.IsOk)
                    return NotFound();

                return result.IsOk ? Ok(result.Result) : BadRequest(result.Message);
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
                var result = await service.GetById(id);

                if (result == null)
                    return NotFound();


                return result.IsOk ? Ok(result) : BadRequest(result.Message);
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
                var result = await service.Update(product);

                return result.IsOk ? Ok(result) : BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }

        }
    }
}
