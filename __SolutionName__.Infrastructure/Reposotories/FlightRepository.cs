using __SolutionName__.Domain.Entities;
using __SolutionName__.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace __SolutionName__.Infrastructure.Reposotories
{
    public class FlightRepository : GenericRepository<Flight>, IFlightRepository
    {
        public FlightRepository(DbContext context) : base(context)
        {
        }
    }
}
