﻿using Dao.Config;
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
                isValid(ProductDTO);

                var product = new Product
                {
                    Name = ProductDTO.Name,
                    Description = ProductDTO.Description,
                    Price = ProductDTO.Price,
                    CategoryId = ProductDTO.CategoryId
                };

                var result = await repository.Create(product);
                await repository.SaveChanges();

                return await Task.FromResult(new Response(result));
            }catch(Exception e)
            {
                return await Task.FromResult(new Response(false, e.Message));

            }


        }

        private void isValid(ProductDTO productDTO)
        {
            if (string.IsNullOrWhiteSpace(productDTO.Name))
            {
                throw new ArgumentException("Name is required");
            }

            if (string.IsNullOrWhiteSpace(productDTO.Description))
            {
                throw new ArgumentException("Description is required");
            }

            if (productDTO.Price <= 0)
            {
                throw new ArgumentException("Price must be greater than 0");
            }

            if (productDTO.CategoryId == Guid.Empty)
            {
                throw new ArgumentException("Invalid CategoryId");
            }
        }

        public async Task<Response> Update(ProductDTO ProductDTO)
        {

            var product = repository.GetById(ProductDTO.Id.Value);
            if (product is null)
                return await Task.FromResult(new Response(false, "product is not found"));


            product.Price = ProductDTO.Price;
            product.Description = ProductDTO.Description;
            product.Name = ProductDTO.Name;

            var result = repository.Update(product);
            await repository.SaveChanges();

            return await Task.FromResult(new Response(result));

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