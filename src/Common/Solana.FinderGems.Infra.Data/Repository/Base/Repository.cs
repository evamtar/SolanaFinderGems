

using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using SharpCompress.Common;
using Solana.FinderGems.Domain.Model.Database.Base;
using Solana.FinderGems.Domain.Repository.Base;
using System;
using System.Linq.Expressions;

namespace Solana.FinderGems.Infra.Data.Repository.Base
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly DbContext _context;
        public Repository(DbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T item)
        {
            await _context.Set<T>().AddAsync(item);
            return item;
        }

        public void Delete(ObjectId id)
        {
            var obj = _context.Set<T>().Where(e => e.ID == id).FirstOrDefault();
            if(obj == null)
                throw new ArgumentNullException(nameof(obj));
            _context.Entry(obj).State = EntityState.Detached;
            _context.Set<T>().Remove(obj);
        }

        public void Delete(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
            _context.Set<T>().Remove(entity);
        }

        public T Update(T item)
        {
            _context.Entry(item).State = EntityState.Detached;
            _context.Set<T>().Update(item);
            return item;
        }

        public virtual async Task<T?> GetAsync(ObjectId id)
        {
            return await this.FindFirstOrDefaultAsync(e => e.ID == id);
        }

        public virtual async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!)
        {
            if (keySelector != null)
                return await _context.Set<T>().Where(predicate).OrderBy(keySelector).ToListAsync();
            else
                return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public virtual async Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!)
        {
            if (keySelector != null)
                return await _context.Set<T>().Where(predicate).OrderBy(keySelector).FirstOrDefaultAsync();
            else
                return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            try 
            {
                _context.Dispose();
            }
            catch{ }
            GC.SuppressFinalize(this);
        }
    }
}
