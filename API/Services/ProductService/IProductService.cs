using Models.DTO;
using Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ProductService
{
    public interface IProductService
    {
        Task<Response> GetAll();
        Task<Response> GetAllPaginated(Pagination pagination);
        Task<Response> GetById(Guid id);
        Task<Response> Create(ProductDTO categoryDTO);
        Task<Response> Update(ProductDTO categoryDTO);
        Task<Response> Delete(ProductDTO categoryDTO);
    }
}
