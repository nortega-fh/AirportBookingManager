using AirportBooking.Enums;

namespace AirportBooking.DTOs;

public record FlightSearchParameters(string OriginCountry, string DestinationCountry,
    DateTime DepartureDate, string? DepartureAirport, string? ArrivalAirport,
    FlightClass? FlightClass, float? MinPrice, float? MaxPrice)
{
    public float? MinPrice { get; init; } = MinPrice ?? 0f;
    public float? MaxPrice { get; init; } = MaxPrice ?? float.MaxValue;
}
