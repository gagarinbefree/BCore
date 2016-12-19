using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BCore.Dal.Repository;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace BCore.Dal.Ef
{
    public class BlogRepository<T> : IRepository<T>  where T : Entity        
    {
        private BlogDbContext _db;

        public BlogRepository(BlogDbContext context)
        {
            _db = context;
        }

        public async Task<int> CreateAsync(T item)
        {
            _db.Set<T>().Add(item);

            return await _db.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(T item)
        {
            _db.Set<T>().Remove(item);

            return await _db.SaveChangesAsync();
        }

        public async Task<T> GetAsync(Guid id)
        {
            return await _db.Set<T>().SingleOrDefaultAsync<T>(m => m.Id == id);
        }

        public async Task<ICollection<T>> GetAllAsync<TOrderKey>(Expression<Func<T, TOrderKey>> orderBy = null
            , bool isDesc = false
            , Expression<Func<T, bool>> where = null
            , int? take = null
            , params Expression<Func<T, object>>[] includes)
        {            
            IQueryable<T> q = isDesc ? _db.Set<T>().OrderByDescending(orderBy) : _db.Set<T>().OrderBy(orderBy);

            if (where != null)
                q = q.Where(where);

            foreach (var include in includes)
            {
                q = q.Include(include);
            }

            if (take != null)
                q = q.Take((int)take);

            return await q.ToListAsync();
        }

        public async Task<int> UpdateAsync(T item)
        {
            _db.Entry(item).State = EntityState.Modified;

            return await _db.SaveChangesAsync();
        }

        public IPagedList<T> GetPage<TOrderKey>(Expression<Func<T, TOrderKey>> orderByDesc = null
            , int page = 1
            , int size = 50
            , params Expression<Func<T, object>>[] includes)
        {
            if (orderByDesc == null)
                throw new Exception("Get With Paging query must be sorted");

            IQueryable<T> q = _db.Set<T>().OrderByDescending(orderByDesc);
            foreach (var include in includes)
            {
                q = q.Include(include);
            }

            return q.ToPagedList(page, size);
        }        
    }
}
