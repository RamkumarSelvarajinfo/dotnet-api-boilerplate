using __SolutionName__.Api.Filters;
using __SolutionName__.Application.DTOs;
using __SolutionName__.Application.DTOs.Flights;
using __SolutionName__.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace __SolutionName__.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(FlightResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Tags = new[] { "flight" }, Summary = "CreateFlight")]
        [ValidationModel(typeof(CreateFlightDto))]
        public async Task<IActionResult> CreateFlight([FromBody] CreateFlightDto model, CancellationToken cancellationToken)
        {
            var flight = await _flightService.CreateFlightAsync(model, cancellationToken);
            return CreatedAtAction(nameof(GetFlightById), new { id = flight.Id }, flight);
        }

        [HttpPatch]
        [ProducesResponseType(typeof(FlightResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "flight" }, Summary = "UpdateFlight")]
        [ValidationModel(typeof(UpdateFlightDto))]
        public async Task<IActionResult> UpdateFlight([FromBody] UpdateFlightDto model, CancellationToken cancellationToken)
        {
            var flight = await _flightService.UpdateFlightAsync(model, cancellationToken);
            return Ok(flight);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "flight" }, Summary = "DeleteFlight")]
        public async Task<IActionResult> DeleteFlight(Guid id, CancellationToken cancellationToken)
        {
            await _flightService.DeleteFlightAsync(id, cancellationToken);
            return Ok();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FlightResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "flight" }, Summary = "GetFlightById")]
        public async Task<IActionResult> GetFlightById(Guid id, CancellationToken cancellationToken)
        {
            var flight = await _flightService.GetFlightByIdAsync(id, cancellationToken);
            if (flight == null)
                return NotFound();

            return Ok(flight);
        }

        [HttpGet("number/{flightNumber}")]
        [ProducesResponseType(typeof(FlightResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "flight" }, Summary = "GetFlightByNumber")]
        public async Task<IActionResult> GetFlightByNumber(string flightNumber, CancellationToken cancellationToken)
        {
            var flight = await _flightService.GetFlightByNumberAsync(flightNumber, cancellationToken);
            if (flight == null)
                return NotFound();

            return Ok(flight);
        }

        [HttpPost("Search")]
        [ProducesResponseType(typeof(SearchResult<FlightResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Tags = new[] { "flight" }, Summary = "SearchFlights")]
        [ValidationModel(typeof(FlightSearchFilterDto))]
        public async Task<IActionResult> SearchFlights([FromBody] FlightSearchFilterDto filter, CancellationToken cancellationToken)
        {
            var result = await _flightService.SearchFlightsAsync(filter, cancellationToken);
            return Ok(result);
        }
    }
}