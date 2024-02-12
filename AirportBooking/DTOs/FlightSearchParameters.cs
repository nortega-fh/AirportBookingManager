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
    public float MinPrice { get; set; } = 0f;
    public float MaxPrice { get; set; } = float.MaxValue;
}
