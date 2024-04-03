using Dao.Config;
using Dao.Context;
using Dao.Repositories.CategoryRepository;

namespace Dao.Repositories.Category
{
    public class CategoryRepository : Repository<Models.entities.Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
