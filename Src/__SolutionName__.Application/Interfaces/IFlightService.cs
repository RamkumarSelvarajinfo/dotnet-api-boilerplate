using __SolutionName__.Application.DTOs;
using __SolutionName__.Application.DTOs.Flights;

namespace __SolutionName__.Application.Interfaces
{
    public interface IFlightService
    {
        Task<FlightResponseDto?> GetFlightByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<FlightResponseDto> CreateFlightAsync(CreateFlightDto dto, CancellationToken cancellationToken = default);

        Task<FlightResponseDto> UpdateFlightAsync(UpdateFlightDto dto, CancellationToken cancellationToken = default);

        Task DeleteFlightAsync(Guid id, CancellationToken cancellationToken = default);

        Task<SearchResult<FlightResponseDto>> SearchFlightsAsync(FlightSearchFilterDto filter, CancellationToken cancellationToken = default);

        Task<FlightResponseDto?> GetFlightByNumberAsync(string flightNumber, CancellationToken cancellationToken = default);
    }
}