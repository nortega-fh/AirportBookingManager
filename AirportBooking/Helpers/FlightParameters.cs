using AirportBooking.Enums;

namespace AirportBooking.Helpers
{
    public record FlightParameters(string OriginCountry, string DestinationCountry,
        DateTime DepartureDate, DateTime? ArrivalDate, string? DepartureAirport, string? ArrivalAirport,
        FlightClass? FlightClass, float MinPrice = 0f, float MaxPrice = float.MaxValue);
}
