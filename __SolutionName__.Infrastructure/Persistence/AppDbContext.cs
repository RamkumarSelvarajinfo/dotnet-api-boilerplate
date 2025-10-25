using __SolutionName__.Domain.Entities;
using __SolutionName__.Infrastructure.Configurations;
using __SolutionName__.Infrastructure.Persistence.DataSeed;
using Microsoft.EntityFrameworkCore;

namespace __SolutionName__.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Flight> Flights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new FlightConfiguration());
            DataSeeder.Seed(modelBuilder);
        }
    }
}
