using Dao.Config;
using Dao.Context;
using Models.entities;

namespace Dao.Repositories.ProductRepository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }
    }
}
