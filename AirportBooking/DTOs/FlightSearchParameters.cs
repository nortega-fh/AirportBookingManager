using AirportBooking.Enums;

namespace AirportBooking.DTOs;

public record FlightSearchParameters(string OriginCountry, string DestinationCountry,
    DateTime DepartureDate, string? DepartureAirport, string? ArrivalAirport,
    FlightClass? FlightClass, float? MinPrice = 0f, float? MaxPrice = float.MaxValue);
