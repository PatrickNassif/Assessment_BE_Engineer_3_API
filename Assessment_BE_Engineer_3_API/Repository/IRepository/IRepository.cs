using System.Linq.Expressions;

namespace Assessment_BE_Engineer_3_API.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T - Category
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        void AddAsync(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
