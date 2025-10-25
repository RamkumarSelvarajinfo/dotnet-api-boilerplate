using __SolutionName__.Application.DTOs.Flights;
using FluentValidation;

namespace __SolutionName__.Application.Validators.Flights
{
    public class UpdateFlightValidator : AbstractValidator<UpdateFlightDto>
    {
        public UpdateFlightValidator()
        {
            RuleFor(x => x.FlightNumber)
                .NotEmpty().WithMessage("Flight number is required.")
                .Length(3, 10).WithMessage("Flight number must be between 3 and 10 characters.");

            RuleFor(x => x.Source)
                .NotEmpty().WithMessage("Origin is required.")
                .Length(3, 50).WithMessage("Origin must be between 3 and 50 characters.");

            RuleFor(x => x.Destination)
                .NotEmpty().WithMessage("Destination is required.")
                .Length(3, 50).WithMessage("Destination must be between 3 and 50 characters.");

            RuleFor(x => x.DepartureTime)
                .GreaterThan(DateTime.UtcNow).WithMessage("Departure time must be in the future.");

            RuleFor(x => x.ArrivalTime)
                .GreaterThan(x => x.DepartureTime).WithMessage("Arrival time must be after the departure time.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");
        }
    }
}
