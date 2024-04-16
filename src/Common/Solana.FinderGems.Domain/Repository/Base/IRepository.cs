using MongoDB.Bson;
using Solana.FinderGems.Domain.Model.Database.Base;
using System.Linq.Expressions;

namespace Solana.FinderGems.Domain.Repository.Base
{
    public interface IRepository<T> : IDisposable where T : Entity
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetAsync(ObjectId id);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
        Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> keySelector = null!);
        

        Task<T> AddAsync(T item);
        T Update(T item);
        void Delete(T entity);
        void Delete(ObjectId id);

        Task SaveChangesAsync();
    }
}
