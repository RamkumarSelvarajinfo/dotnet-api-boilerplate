namespace __SolutionName__.Application.DTOs.Flights
{
    public class FlightSearchFilterDto
    {
        public string? FlightNumber { get; set; }
        public string? Source { get; set; }
        public string? Destination { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
