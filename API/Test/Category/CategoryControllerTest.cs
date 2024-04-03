using API.Controllers;
using Dao.Repositories.CategoryRepository;
using Microsoft.AspNetCore.Mvc;
using Models.entities;
using Models.Utils;
using Moq;
using Services.CategoryService;
using System.Xml.Linq;

namespace Test.Category
{
    public class CategoryControllerTest
    {
        private readonly Mock<ICategoryService> mockService;
        private readonly Mock<ICategoryRepository> mockCategoryRepository;
        

        public CategoryControllerTest()
        {
            mockService = new Mock<ICategoryService>();
        }

        [Fact]
        public async Task Get_Returns_OkResult_With_Data()
        {
            // Arrange
            var listCategory = new List<Models.entities.Category>
            {
                new Models.entities.Category { }
            };
            mockService.Setup(service => service.GetAll()).ReturnsAsync(new Response(listCategory));

            var controller = new CategoryController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<Response>(okResult.Value);

            Assert.Equal(listCategory, model.Result);
        }
    }
}