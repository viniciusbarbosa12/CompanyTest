using Dao.Config;
using Dao.Repositories.CategoryRepository;
using Dao.Repositories.ProductRepository;
using Models.DTO;
using Models.entities;
using Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ProductService
{
    public class ProductService : IProductService
    {

        private readonly IProductRepository repository;

        public ProductService(IProductRepository repository)
        {
            this.repository = repository;
        }


        public async Task<Response> GetAll()
        {
            var products = repository.GetAll(item => item.Active && !item.DeletedAt.HasValue);

            if (products is null)
                return await Task.FromResult(new Response(false, "product is not found"));

            return await Task.FromResult(new Response(products));
        }


        public async Task<Response> GetAllPaginated(Pagination pagination)
        {
            var products = repository.GetAll(item => item.Active == true && !item.DeletedAt.HasValue, pagination, item => item.Category);
            return await Task.FromResult(new Response(new
            {
                Itens = products.Itens.ToList().Select(item => new {
                    item.Name,
                    item.Price,
                    item.Description,
                    Category = item.Category?.Name,
                    item.Id
                }),
                products.TotalItens,
                products.TotalPages
            }));
        }


        public async Task<Response> GetById(Guid id)
        {
            var product = repository.GetById(id);
            return await Task.FromResult(new Response(product));
        }

        public async Task<Response> Create(ProductDTO ProductDTO)
        {
            try
            {
                ValidateProductDTO(ProductDTO);

                var product = MapProductDTOToProduct(ProductDTO);

                var createdProduct = await repository.Create(product);
                await repository.SaveChanges();

                return new Response(createdProduct);
            }
            catch (Exception e)
            {
                return new Response(false, e.Message);
            }
        }

        private void ValidateProductDTO(ProductDTO productDTO)
        {
            if (string.IsNullOrWhiteSpace(productDTO.Name))
            {
                throw new ArgumentException("Name is required", nameof(productDTO.Name));
            }

            if (string.IsNullOrWhiteSpace(productDTO.Description))
            {
                throw new ArgumentException("Description is required", nameof(productDTO.Description));
            }

            if (productDTO.Price <= 0)
            {
                throw new ArgumentException("Price must be greater than 0", nameof(productDTO.Price));
            }

            if (productDTO.CategoryId == Guid.Empty)
            {
                throw new ArgumentException("Invalid CategoryId", nameof(productDTO.CategoryId));
            }
        }

        private Product MapProductDTOToProduct(ProductDTO productDTO)
        {
            return new Product
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                CategoryId = productDTO.CategoryId
            };
        }

        public async Task<Response> Update(ProductDTO productDTO)
        {
            try
            {
                if (productDTO == null || !productDTO.Id.HasValue)
                {
                    return new Response(false, "ProductDTO or its Id is null");
                }

                var existingProduct = repository.GetById(productDTO.Id.Value);
                if (existingProduct == null)
                {
                    return new Response(false, "Product not found");
                }

                existingProduct.Price = productDTO.Price;
                existingProduct.Description = productDTO.Description;
                existingProduct.Name = productDTO.Name;

                repository.Update(existingProduct);
                await repository.SaveChanges();
                return new Response(existingProduct);
            }
            catch (Exception e)
            {
                return new Response(false, e.Message);
            }
        }


        public async Task<Response> Delete(ProductDTO ProductDTO)
        {
            var product = repository.GetById(ProductDTO.Id.Value);
            if (product is null)
                return await Task.FromResult(new Response(false, "product is not found"));

            var result = repository.Delete(product);
            await repository.SaveChanges();

            return await Task.FromResult(new Response(result));

        }
    }
}
