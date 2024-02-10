using AirportBooking.Enums;

namespace AirportBooking.DTOs;

public record BookingParameters(string? FlightNumber, float? MinPrice, float? MaxPrice, string? DepartureCountry,
    string? DestinationCountry, DateTime? DepartureDate, string? DepartureAirport, string? ArrivalAirport,
    string? Passenger, FlightClass? FlightClass, BookingType? BookingType, BookingStatus? Status);
