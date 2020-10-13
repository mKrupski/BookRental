using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookRental.Infrastructure.Repositories.IRepository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAllWithProperties(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null
        );
        Task<T> GetFirstOrDefaultWithProperties(Expression<Func<T, bool>> filter = null, string includeProperties = null);
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task DeleteById(int id);
    }
}
