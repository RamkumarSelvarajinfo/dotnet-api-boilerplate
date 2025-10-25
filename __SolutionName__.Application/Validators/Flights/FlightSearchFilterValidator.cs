using __SolutionName__.Application.DTOs.Flights;
using FluentValidation;

namespace __SolutionName__.Application.Validators.Flights
{
    public class FlightSearchFilterValidator : AbstractValidator<FlightSearchFilterDto>
    {
        public FlightSearchFilterValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Page number must be greater than 0.");
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
        }
    }
}
