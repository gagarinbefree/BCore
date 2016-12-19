using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BCore.Dal.Repository
{
    public interface IRepository<T>
    {
        Task<int> CreateAsync(T item);
        Task<int> DeleteAsync(T item);
        Task<ICollection<T>> GetAllAsync<TOrderKey>(Expression<Func<T, TOrderKey>> orderBy = null
            , bool isDesc = false
            , Expression<Func<T, bool>> where = null
            , int? take = null
            , params Expression<Func<T, object>>[] includes);
        Task<T> GetAsync(Guid id);
        Task<int> UpdateAsync(T item);
        IPagedList<T> GetPage<TOrderKey>(Expression<Func<T, TOrderKey>> orderByDesc = null, int page = 1, int size = 50, params Expression<Func<T, object>>[] includes);        
    }
}