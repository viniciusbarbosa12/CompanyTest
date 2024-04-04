using Dao.Config;
using Dao.Repositories.CategoryRepository;
using Models.DTO;
using Models.entities;
using Models.Utils;

namespace Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository repository;

        public CategoryService(ICategoryRepository repository)
        {
            this.repository = repository;
        }


        public async Task<Response> GetAll()
        {
            var categories = repository.GetAll(item => item.Active && !item.DeletedAt.HasValue);

            if (categories is null)
                return await Task.FromResult(new Response(false, "Category is not found"));

            return await Task.FromResult(new Response(categories));
        }


        public async Task<Response> GetAllPaginated(Pagination pagination)
        {
            var categories = repository.GetAll(item => item.Active == true && !item.DeletedAt.HasValue, pagination);
            return await Task.FromResult(new Response(new
            {
                Itens = categories.Itens.ToList(),
                categories.TotalItens,
                categories.TotalPages
            }));
        }


        public async Task<Response> GetById(Guid id)
        {
            var category = repository.GetById(id);
            return await Task.FromResult(new Response(category));
        }

        public async Task<Response> Create(CategoryDTO categoryDTO)
        {
            ValidateCategoryDTO(categoryDTO);

            var category = new Category
            {
                Name = categoryDTO.Name,
                Code = categoryDTO.Code,
            };

            var result = await repository.Create(category);
            await repository.SaveChanges();

            return await Task.FromResult(new Response(result));

        }


        public async Task<Response> Update(CategoryDTO categoryDTO)
        {
            ValidateCategoryDTO(categoryDTO);

            if (!categoryDTO.Id.HasValue)
            {
                throw new ArgumentException("Category is invalid");
            }

            var category = repository.GetById(categoryDTO.Id.Value);
            if (category is null)
                return await Task.FromResult(new Response(false, "Category is not found"));


            category.Code = categoryDTO.Code;
            category.Name = categoryDTO.Name;

            var result = repository.Update(category);
            await repository.SaveChanges();

            return await Task.FromResult(new Response(result));

        }

        private void ValidateCategoryDTO(CategoryDTO categoryDTO)
        {
            if (categoryDTO is null)
            {
                throw new ArgumentException("Product is invalid");
            }

            if (string.IsNullOrWhiteSpace(categoryDTO.Name))
            {
                throw new ArgumentException("Name is required", nameof(categoryDTO.Name));
            }

            if (string.IsNullOrWhiteSpace(categoryDTO.Code))
            {
                throw new ArgumentException("Code is required", nameof(categoryDTO.Code));
            }
        }

        public async Task<Response> Delete(CategoryDTO categoryDTO)
        {
            var category = repository.GetById(categoryDTO.Id.Value);
            if (category is null)
                return await Task.FromResult(new Response(false, "Category is not found"));

            var result = repository.Delete(category);
            await repository.SaveChanges();

            return await Task.FromResult(new Response(result));

        }
    }
}
