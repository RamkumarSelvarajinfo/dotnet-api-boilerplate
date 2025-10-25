using AutoMapper;
using __SolutionName__.Application.DTOs.Flights;
using __SolutionName__.Domain.Entities;

namespace __SolutionName__.Application.MappingProfiles
{
    public class FlightMappingProfile : Profile
    {
        public FlightMappingProfile()
        {
            CreateMap<Flight, FlightResponseDto>();
            CreateMap<CreateFlightDto, Flight>();
        }
    }
}
