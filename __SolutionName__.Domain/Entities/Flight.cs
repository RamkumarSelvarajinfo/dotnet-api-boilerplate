using __SolutionName__.Domain.Entities.Base;

namespace __SolutionName__.Domain.Entities
{
    public class Flight : BaseEntity
    {
        public Guid Id { get; set; }
        public string FlightNumber { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
    }
}
