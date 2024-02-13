using AirportBooking.Enums;

namespace AirportBooking.Models;

public class Flight
{
    public string Number { get; set; } = string.Empty;
    public string OriginCountry { get; set; } = string.Empty;
    public string DestinationCountry { get; set; } = string.Empty;
    public DateTime DepartureDate { get; set; } = DateTime.Now;
    public DateTime ArrivalDate { get; set; } = DateTime.Now;
    public string OriginAirport { get; set; } = string.Empty;
    public string DestinationAirport { get; set; } = string.Empty;
    public SortedDictionary<FlightClass, decimal> ClassPrices { get; set; } = [];
}
