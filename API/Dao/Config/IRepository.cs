using Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Config
{
    public interface IRepository<T>
    {
        //I have implemented four methods for retrieving all data from the database
        IQueryable<T> GetAll();

        //Class Expression in C#.NET encapsulates lambda expressions
        //enabling the creation of versatile and reusable filters for querying data
        IQueryable<T> GetAll(Expression<Func<T, bool>> expression);

        //Pagination is a utility class designed to streamline the implementation of paginated queries
        //enhancing efficiency and simplifying requests for retrieving all items
        (IQueryable<T> Itens, int TotalPaginas, int TotalItens) GetAll(Pagination pagination);

        //Implemented a new GetAll method utilizing expressions to facilitate custom filters and pagination
        //enhancing flexibility and efficiency in data retrieval operations
        (IQueryable<T> Itens, int TotalPaginas, int TotalItens) GetAll(Expression<Func<T, bool>> expression, Pagination pagination);


        T GetById(Guid id);

        Task<T> Create(T entity);
        Task CreateRangeAsync(ICollection<T> entity);
        T Update(T entity);
        void DeleteRange(Expression<Func<T, bool>> predicate);
        T Delete(T entity);
        Task SaveChanges();
    }
}
