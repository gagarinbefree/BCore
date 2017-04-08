using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Data.SqlClient;

namespace BCore.Dal.Ef
{
    public class BlogRepository<T> : IRepository<T> where T : Entity
    {
        private BlogDbContext _db;

        public BlogRepository(BlogDbContext context)
        {
            _db = context;
        }
       
        public async Task<Guid> CreateAsync(T item)
        {
            var entity = item as Entity;

            if (entity == null)
                throw new Exception(String.Format("{0} is not Entity", item.GetType()));

            _db.Set<T>().Add(item);

            await _db.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<int> DeleteAsync(T item)
        {
            _db.Set<T>().Remove(item);

            return await _db.SaveChangesAsync();            
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            T item = default(T);
            item = Activator.CreateInstance<T>();
            item.Id = id;

            _db.Set<T>().Attach(item);
            _db.Set<T>().Remove(item);

            return await _db.SaveChangesAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> q = _db.Set<T>();
            foreach (var include in includes)
            {
                q = q.Include(include);
            }

            return await q.Where(where).SingleOrDefaultAsync();
        }

        public async Task<T> GetAsync(Guid id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> q = _db.Set<T>();
            foreach (var include in includes)
            {
                q = q.Include(include);
            }

            return await q.Where(f => f.Id == id).SingleOrDefaultAsync();            
        }

        public async Task<object> GetValueAsync(Guid id, Expression<Func<T, object>> selector)
        {
            IQueryable<T> q = _db.Set<T>();

            return await q.Where(f => f.Id == id).Select(selector).SingleOrDefaultAsync();
        }
        
        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> where = null
            , int? skip = default(int?)
            , int? take = default(int?)
            , params Expression<Func<T, object>>[] includes)
        {
            return await _getAll(where, skip, take, includes).ToListAsync();            
        }

        public async Task<ICollection<T>> GetAllAsync<TOrderKey>(Expression<Func<T, TOrderKey>> orderBy = null,
            SortOrder sort = SortOrder.Unspecified,
            Expression<Func<T, bool>> where = null,
            int? skip = default(int?),
            int? take = default(int?),
            params Expression<Func<T, object>>[] includes)
        {
            return await _getAll(where, skip, take, includes).ToListAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> where = null
            , int? skip = default(int?)
            , int? take = default(int?)
            , params Expression<Func<T, object>>[] includes)
        {
            ICollection<T> items = await _getAll(where, skip, take, includes).ToListAsync();

            return items.Count();
        }

        public async Task<int> CountAsync<TOrderKey>(Expression<Func<T, TOrderKey>> orderBy = null,
            SortOrder sort = SortOrder.Unspecified,
            Expression<Func<T, bool>> where = null,
            int? skip = default(int?),
            int? take = default(int?),
            params Expression<Func<T, object>>[] includes)
        {
            ICollection<T> items = await _getAll(where, skip, take, includes).ToListAsync();

            return items.Count();
        }

        //public async Task<ICollection<T>> GetAllAsync<TOrderKey>(Expression<Func<T, TOrderKey>> orderBy = null
        //    , bool isDesc = false
        //    , Expression<Func<T, bool>> where = null
        //    , int? take = null
        //    , params Expression<Func<T, object>>[] includes)
        //{
        //    IQueryable<T> q = _getAll(where, null, take, includes);

        //    if (orderBy != null)
        //        q = isDesc ? _db.Set<T>().OrderByDescending(orderBy) : _db.Set<T>().OrderBy(orderBy);           

        //    return await q.ToListAsync();            
        //}

        //public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> where = null, int? take = null)
        //{
        //    IQueryable<T> q = _getAll(where, null, take, null);            

        //    return await q.ToListAsync();
        //}

        //public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> where = null, int? take = default(int?), params Expression<Func<T, object>>[] includes)
        //{
        //    IQueryable<T> q = _getAll(where, null, take, includes);

        //    return await q.ToListAsync();
        //}

        //public async Task<ICollection<T>> GetAllAsync<TOrderKey>(Expression<Func<T, TOrderKey>> orderBy, bool isDesc, int? skip, int? take, params Expression<Func<T, object>>[] includes)
        //{
        //    IQueryable<T> q = _getAll(null, skip, take, includes);

        //    if (orderBy != null)
        //        q = isDesc ? _db.Set<T>().OrderByDescending(orderBy) : _db.Set<T>().OrderBy(orderBy);

        //    return await q.ToListAsync();
        //}

        //public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> where = null, int? skip = default(int?), int? take = default(int?), params Expression<Func<T, object>>[] includes)
        //{
        //    IQueryable<T> q = _db.Set<T>();

        //    if (where != null)
        //        q = q.Where(where);

        //    if (skip != null)
        //        q = q.Skip((int)skip);

        //    if (take != null)
        //        q = q.Take((int)take);

        //    foreach (var include in includes)
        //    {
        //        q = q.Include(include);
        //    }

        //    return await q.ToListAsync();
        //}

        //public async Task<ICollection<T>> GetAllAsync()
        //{
        //    IQueryable<T> q = _db.Set<T>();

        //    return await q.ToListAsync();
        //}

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

        private IQueryable<T> _getAll(IQueryable<T> items = null,
            Expression<Func<T, bool>> where = null,
            int? skip = null,
            int? take = null,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> res = items ?? _db.Set<T>();

            if (where != null)
                res = res.Where(where);

            if (skip != null)
                res = res.Skip((int)skip);

            if (take != null)
                res = res.Take((int)take);

            foreach (var include in includes)
            {
                res = res.Include(include);
            }

            return res;
        }

        private IQueryable<T> _getAll<TOrderKey>(Expression<Func<T, TOrderKey>> orderBy = null,
            SortOrder sort = SortOrder.Unspecified,
            Expression<Func<T, bool>> where = null,
            int? skip = null,
            int? take = null,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> items = _db.Set<T>();

            if (orderBy != null)
                items = sort == SortOrder.Descending ? items.OrderByDescending(orderBy) : items.OrderBy(orderBy);

            return _getAll(items, where, skip, take, includes);
        }
    }
}
