using __SolutionName__.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace __SolutionName__.Infrastructure.Configurations
{
    public class FlightConfiguration : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {
            builder.ToTable("Flights");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.FlightNumber)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(f => f.Source)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(f => f.Destination)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(f => f.DepartureTime)
                .IsRequired();

            builder.Property(f => f.ArrivalTime)
                .IsRequired();

            builder.Property(f => f.Price)
                .HasPrecision(18, 2) // Configure precision and scale for decimal
                .IsRequired();
        }
    }
}
