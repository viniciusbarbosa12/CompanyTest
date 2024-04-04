using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Models.Utils;
using Moq;
using Services.ProductService;

namespace Test.ProductControllerTest
{
    public class ProductControllerTest
    {
        private readonly Mock<IProductService> mockService;


        public ProductControllerTest()
        {
            mockService = new Mock<IProductService>();
        }

        [Fact]
        public async Task Get_Returns_OkResult_With_Data()
        {
            // Arrange
            var listProduct = new List<Models.entities.Product>
            {
                new Models.entities.Product { }
            };
            mockService.Setup(service => service.GetAll()).ReturnsAsync(new Response(listProduct));

            var controller = new ProductController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<Response>(okResult.Value);

            Assert.Equal(listProduct, model.Result);
        }

        [Fact]
        public async Task Get_Returns_BadRequestResult_With_Data()
        {
            // Arrange
            mockService.Setup(service => service.GetAll()).ReturnsAsync(new Response(false, "product is not found"));

            var controller = new ProductController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var okResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<Response>(okResult.Value);

            Assert.False(model.IsOk);
            Assert.Equal("product is not found", model.Message);
        }

        [Fact]
        public async Task Get_Returns_OkResult_Create()
        {
            // Arrange
            var product = new ProductDTO
            {
                Name = "Test",
                Description = "Test",
                CategoryId  = Guid.NewGuid()
                
            };
            mockService.Setup(service => service.Create(product)).ReturnsAsync(new Response());

            var controller = new ProductController(mockService.Object);

            // Act
            var result = await controller.Create(product);

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<Response>(okResult.Value);

            Assert.True(model.IsOk);
        }


        [Fact]
        public async Task Get_Returns_BadRequestResult_Create()
        {
            // Arrange
            
            mockService.Setup(service => service.Create(null)).ReturnsAsync(new Response());

            var controller = new ProductController(mockService.Object);

            // Act
            var result = await controller.Create(null);

            // Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<string>(okResult.Value);

            Assert.Equal("Product params is null", model);
        }

        [Fact]
        public async Task Get_Returns_BadRequestResult_Create_Validations()
        {
            // Arrange
            var invalidProduct = new ProductDTO(); // Create an invalid ProductDTO object with missing required fields

            // Setup the mock service to return a Response indicating invalid parameters
            mockService.Setup(service => service.Create(invalidProduct))
                       .ReturnsAsync(new Response(false, "Name is required"));

            var controller = new ProductController(mockService.Object);

            // Act
            var result = await controller.Create(invalidProduct);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var errorMessage = Assert.IsAssignableFrom<string>(badRequestResult.Value);

            Assert.Equal("Name is required", errorMessage);
        }
    }
}
