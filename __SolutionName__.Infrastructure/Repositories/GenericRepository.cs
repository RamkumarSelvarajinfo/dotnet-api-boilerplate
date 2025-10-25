using __SolutionName__.Domain.Entities.Base;
using __SolutionName__.Domain.Interfaces.Repositories;
using __SolutionName__.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading;

namespace __SolutionName__.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(object id, bool isTracking = true, CancellationToken cancellationToken = default)
        {
            if (isTracking)
                return await _dbSet.FindAsync(new object[] { id }, cancellationToken);

            // For no tracking with find, EF doesn't support AsNoTracking for FindAsync — so we use FirstOrDefaultAsync
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(e => EF.Property<object>(e, "Id") == id, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool isTracking = true, CancellationToken cancellationToken = default)
        {
            var query = isTracking ? _dbSet : _dbSet.AsNoTracking();
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool isTracking = true, CancellationToken cancellationToken = default)
        {
            var query = isTracking ? _dbSet.Where(predicate) : _dbSet.Where(predicate).AsNoTracking();
            return await query.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null, bool isTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (predicate != null)
                query = query.Where(predicate);

            return isTracking ? query : query.AsNoTracking();
        }
    }
}