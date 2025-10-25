using __SolutionName__.Application.DTOs;
using __SolutionName__.Application.DTOs.Flights;

namespace __SolutionName__.Application.Interfaces
{
    public interface IFlightService
    {
        Task<FlightResponseDto?> GetFlightByIdAsync(Guid id);
        Task<FlightResponseDto> CreateFlightAsync(CreateFlightDto dto);
        Task<FlightResponseDto> UpdateFlightAsync(UpdateFlightDto dto);
        Task DeleteFlightAsync(Guid id);
        Task<SearchResult<FlightResponseDto>> SearchFlightsAsync(FlightSearchFilterDto filter);

        Task<FlightResponseDto?> GetFlightByNumberAsync(string flightNumber);
    }
}
