using __SolutionName__.Domain.Entities;
using __SolutionName__.Domain.Interfaces.Repositories;
using __SolutionName__.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace __SolutionName__.Infrastructure.Repositories
{
    public class FlightRepository : GenericRepository<Flight>, IFlightRepository
    {
        public FlightRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Flight?> GetFlightByNumberAsync(string flightNumber)
        {
            return await _dbSet.Where(x => x.FlightNumber == flightNumber).FirstOrDefaultAsync();
        }
    }
}
