using __SolutionName__.Domain.Entities.Base;
using System.Linq.Expressions;

namespace __SolutionName__.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(object id, bool isTracking = true, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetAllAsync(bool isTracking = true, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool isTracking = true, CancellationToken cancellationToken = default);

        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        void Update(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

        IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null, bool isTracking = true);
    }
}