using Core.Entities;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
    public interface IEntityRepository<T>
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? orderBy = null,bool desc = false);
        Task<PagedResult<T>> GetAllAsyncPageResult(int page = 1,int pageSize = 20,Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? orderBy = null,bool desc = false);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter);
        Task AddAsync(T entity);
        Task AddRangeAsync(List<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(List<T> entities);
    }
}
