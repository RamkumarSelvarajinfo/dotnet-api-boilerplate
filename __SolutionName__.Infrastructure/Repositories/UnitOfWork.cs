using __SolutionName__.Domain.Entities.Base;
using __SolutionName__.Domain.Interfaces.Repositories;
using __SolutionName__.Infrastructure.Persistence;
using __SolutionName__.Infrastructure.Repositories;
using System.Collections.Concurrent;

namespace __SolutionName__.Infrastructure.Reposotories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly ConcurrentDictionary<Type, object> _repositories = new();

        private IFlightRepository _flightRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            if (!_repositories.ContainsKey(typeof(T)))
            {
                var repository = new GenericRepository<T>(_context);
                _repositories.TryAdd(typeof(T), repository);
            }

            return (IGenericRepository<T>)_repositories[typeof(T)];
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IFlightRepository Flights => _flightRepository ??= new FlightRepository(_context);
    }
}
