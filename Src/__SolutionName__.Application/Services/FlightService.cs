using AutoMapper;
using __SolutionName__.Application.DTOs;
using __SolutionName__.Application.DTOs.Flights;
using __SolutionName__.Application.Interfaces;
using __SolutionName__.Domain.Entities;
using __SolutionName__.Domain.Exceptions;
using __SolutionName__.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace __SolutionName__.Application.Services
{
    public class FlightService : IFlightService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private readonly ILogger<FlightService> _logger;

        public FlightService(
            IUnitOfWork unitOfWork,
            ICacheService cacheService,
            IMapper mapper,
            ILogger<FlightService> logger)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FlightResponseDto?> GetFlightByIdAsync(Guid flightId, CancellationToken cancellationToken = default)
        {
            string cacheKey = CacheKeys.GetFlightById(flightId);

            var cachedFlight = await _cacheService.GetAsync<FlightResponseDto>(cacheKey);
            if (cachedFlight != null)
                return cachedFlight;

            var flight = await _unitOfWork.Repository<Flight>()
                                          .GetByIdAsync(flightId, isTracking: false, cancellationToken);

            if (flight == null)
                throw new BusinessException($"Flight Id: '{flightId}' not found.", HttpStatusCode.NotFound);

            var flightModel = _mapper.Map<FlightResponseDto>(flight);

            await _cacheService.SetAsync(cacheKey, flightModel, TimeSpan.FromMinutes(10));

            return flightModel;
        }

        public async Task<FlightResponseDto> CreateFlightAsync(CreateFlightDto dto, CancellationToken cancellationToken = default)
        {
            var flight = _mapper.Map<Flight>(dto);
            flight.Id = Guid.NewGuid();

            await _unitOfWork.Repository<Flight>().AddAsync(flight, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            var flightModel = _mapper.Map<FlightResponseDto>(flight);

            await _cacheService.RemoveAsync(CacheKeys.GetAllFlights());

            return flightModel;
        }

        public async Task<FlightResponseDto> UpdateFlightAsync(UpdateFlightDto dto, CancellationToken cancellationToken = default)
        {
            var flight = await _unitOfWork.Repository<Flight>()
                                          .GetByIdAsync(dto.Id, isTracking: true, cancellationToken);
            if (flight == null)
                throw new BusinessException("Flight not found.", HttpStatusCode.NotFound);

            _mapper.Map(dto, flight);
            await _unitOfWork.SaveChangesAsync();

            string cacheKey = CacheKeys.GetFlightById(dto.Id);
            var updatedFlight = _mapper.Map<FlightResponseDto>(flight);
            await _cacheService.SetAsync(cacheKey, updatedFlight, TimeSpan.FromMinutes(10));

            return updatedFlight;
        }

        public async Task DeleteFlightAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var flight = await _unitOfWork.Repository<Flight>().GetByIdAsync(id, isTracking: true, cancellationToken);
            if (flight == null)
                throw new BusinessException("Flight not found.", HttpStatusCode.NotFound);

            _unitOfWork.Repository<Flight>().Remove(flight);
            await _unitOfWork.SaveChangesAsync();

            string cacheKey = CacheKeys.GetFlightById(id);
            await _cacheService.RemoveAsync(cacheKey);
            await _cacheService.RemoveAsync(CacheKeys.GetAllFlights());
        }

        public async Task<SearchResult<FlightResponseDto>> SearchFlightsAsync(FlightSearchFilterDto filter, CancellationToken cancellationToken = default)
        {
            var flightsQuery = _unitOfWork.Repository<Flight>().Query(f =>
                (string.IsNullOrEmpty(filter.FlightNumber) || f.FlightNumber == filter.FlightNumber) &&
                (string.IsNullOrEmpty(filter.Source) || f.Source == filter.Source) &&
                (string.IsNullOrEmpty(filter.Destination) || f.Destination == filter.Destination) &&
                (!filter.DepartureDate.HasValue || f.DepartureTime.Date == filter.DepartureDate.Value.Date) &&
                (!filter.ArrivalDate.HasValue || f.ArrivalTime.Date == filter.ArrivalDate.Value.Date),
                isTracking: false // read-only search, no tracking for perf
            );

            var totalCount = await flightsQuery.CountAsync(cancellationToken);

            var flights = await flightsQuery
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync(cancellationToken);

            var flightDtos = _mapper.Map<IEnumerable<FlightResponseDto>>(flights);

            return new SearchResult<FlightResponseDto>
            {
                Items = flightDtos,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public async Task<FlightResponseDto?> GetFlightByNumberAsync(string flightNumber, CancellationToken cancellationToken = default)
        {
            // Using custom repository through UnitOfWork
            var flight = await _unitOfWork.Flights.GetFlightByNumberAsync(flightNumber);
            if (flight == null)
                throw new BusinessException($"Flight with number '{flightNumber}' not found.", HttpStatusCode.NotFound);

            return _mapper.Map<FlightResponseDto>(flight);
        }
    }
}