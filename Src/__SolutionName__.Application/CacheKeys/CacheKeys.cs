using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace __SolutionName__.Application
{
    public static class CacheKeys
    {
        public static string GetFlightById(Guid flightId) => $"Flight_{flightId}";
        public static string GetAllFlights() => "Flights_All";
        public static string GetBookingById(Guid bookingId) => $"Booking_{bookingId}";
    }
}
