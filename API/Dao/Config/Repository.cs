using Dao.Context;
using Microsoft.EntityFrameworkCore;
using Models.Utils;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Dao.Config
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected AppDbContext Context { get; set; }

        public Repository(AppDbContext context)
        {
            Context = context;
        }

        public IQueryable<T> GetAll()
        {
            return Context.Set<T>().ToList().AsQueryable();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return Context.Set<T>().Where(expression).ToList().AsQueryable();
        }

        public T GetById(Guid id)
        {
            var entidade = GetAll(item => item.Id == id);
            if (entidade.Any())
            {
                return entidade.First();
            }
            else
            {
                throw new InvalidOperationException($"Não existe nenhuma entidade {typeof(T)} com ID={id}");
            }
        }

        public (IQueryable<T> Itens, int TotalPages, int TotalItens) GetAll(Pagination pagination)
        {
            var result = Context.Set<T>();
            return ApplyParams(pagination, result);
        }

        public (IQueryable<T> Itens, int TotalPages, int TotalItens) GetAll(Expression<Func<T, bool>> expression,
            Pagination pagination, params Expression<Func<T, object>>[]? includes)
        {
            var result = Context.Set<T>().Where(expression);
            result = includes?.Aggregate(result, (current, includeProperty) => current.Include(includeProperty));
            return ApplyParams(pagination, result);
        }

        private (IQueryable<T> Itens, int TotalPages, int TotalItens) ApplyParams(Pagination pagination, IQueryable<T> result)
        {
            var lista = ApplyOrder(result, pagination.Order, pagination.Desc);
            (IQueryable<T> Itens, int TotalPages, int TotalItens) = ApplyPagination(lista, pagination.Page, pagination.PageSize);
            return (Itens.ToList().AsQueryable(), TotalPages, TotalItens);
        }

        private (IQueryable<T> Items, int TotalPages, int TotalItens) ApplyPagination(IQueryable<T> lista, int pagina, int tamanhoPagina)
        {
            if (pagina == 0) pagina = 1;
            if (tamanhoPagina == 0) tamanhoPagina = 10;

            var totalItens = lista.Count();
            var TotalPages = (double)totalItens / tamanhoPagina;
            TotalPages = Math.Ceiling(TotalPages);

            pagina = pagina < 1 ? 1 : pagina;

            var skip = (pagina - 1) * tamanhoPagina;
            return (lista.Skip(skip).Take(tamanhoPagina), (int)TotalPages, totalItens);
        }


        //This method is utilized to apply sorting order to our list
        //It receives a list of objects along with the desired order and
        //whether it should be in ascending or descending order
        private IQueryable<T> ApplyOrder(IQueryable<T> list, string order, bool desc)
        {
            if (string.IsNullOrWhiteSpace(order))
                return list;

            var methodName = desc ? "OrderByDescending" : "OrderBy";

            var props = order.Split('.');
            var type = typeof(T);
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                System.Reflection.PropertyInfo pi = type.GetProperty(prop.FirstCharToUpper());
                if (pi == null)
                {
                    throw new InvalidOperationException(
                        $"Impossible order by property {prop}. " +
                        $"Property not found {typeof(T).Name}"
                    );
                }
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            var result = typeof(Queryable).GetMethods().FirstOrDefault(
                method => method.Name == methodName
                    && method.IsGenericMethodDefinition
                    && method.GetGenericArguments().Length == 2
                    && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { list, lambda });

            return (IOrderedQueryable<T>)result;
        }

        public async Task<T> Create(T entity)
        {
            entity.CreatedAt = DateTime.Now;
            await Context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task CreateRangeAsync(ICollection<T> entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await Context.Set<T>().AddRangeAsync(entity);
        }

        public T Update(T entity)
        {
            entity.UpdatedAt = DateTime.Now;
            Context.Set<T>().Update(entity);
            return entity;
        }

        public T Delete(T entity)
        {
            entity.DeletedAt = DateTime.Now;
            Context.Set<T>().Update(entity);
            return entity;
        }


        public void DeleteRange(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var entities = Context.Set<T>().Where(predicate);

            Context.Set<T>().RemoveRange(entities);
        }

        public async Task SaveChanges()
        {
            await Context.SaveChangesAsync();
        }

    }
}
