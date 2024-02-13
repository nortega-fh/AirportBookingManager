using AirportBooking.Enums;

namespace AirportBooking.DTOs;

public class FlightSearchParameters
{
    public string? OriginCountry { get; set; }
    public string? DestinationCountry { get; set; }
    public DateTime? DepartureDate { get; set; }
    public string? DepartureAirport { get; set; }
    public string? ArrivalAirport { get; set; }
    public FlightClass? FlightClass { get; set; }
    public decimal MinPrice { get; set; } = 0m;
    public decimal MaxPrice { get; set; } = decimal.MaxValue;
}
