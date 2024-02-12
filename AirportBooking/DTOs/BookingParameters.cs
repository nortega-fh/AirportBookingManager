using AirportBooking.Enums;

namespace AirportBooking.DTOs;

public class BookingParameters
{
    public string? FlightNumber { get; set; }
    public string? Passenger { get; set; }
    public BookingType? BookingType { get; set; }
    public BookingStatus? Status { get; set; }
    public FlightSearchParameters? FlightSearchParameters { get; set; }
}
