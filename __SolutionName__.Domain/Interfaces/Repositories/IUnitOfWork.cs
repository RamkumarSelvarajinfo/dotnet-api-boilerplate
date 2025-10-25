using __SolutionName__.Domain.Entities.Base;

namespace __SolutionName__.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> SaveChangesAsync();

        IFlightRepository Flights { get; }
    }
}
