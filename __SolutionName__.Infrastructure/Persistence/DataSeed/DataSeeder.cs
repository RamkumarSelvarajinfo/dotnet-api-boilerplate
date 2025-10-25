using __SolutionName__.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace __SolutionName__.Infrastructure.Persistence.DataSeed
{
    public static class DataSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flight>().HasData(
                new Flight
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), // Static GUID
                    FlightNumber = "FL123",
                    Source = "New York",
                    Destination = "London",
                    DepartureTime = new DateTime(2023, 10, 1, 10, 0, 0, DateTimeKind.Utc), // Static DateTime
                    ArrivalTime = new DateTime(2023, 10, 1, 18, 0, 0, DateTimeKind.Utc), // Static DateTime
                    Price = 500
                },
                new Flight
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Static GUID
                    FlightNumber = "FL456",
                    Source = "Los Angeles",
                    Destination = "Tokyo",
                    DepartureTime = new DateTime(2023, 10, 2, 12, 0, 0, DateTimeKind.Utc), // Static DateTime
                    ArrivalTime = new DateTime(2023, 10, 2, 22, 0, 0, DateTimeKind.Utc), // Static DateTime
                    Price = 800
                }
            );
        }
    }
}
