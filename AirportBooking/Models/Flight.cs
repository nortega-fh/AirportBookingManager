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
    public SortedDictionary<FlightClass, float> ClassPrices { get; set; } = [];

    public string PricesToString()
    {
        string format = "";
        foreach (KeyValuePair<FlightClass, float> f in ClassPrices)
        {
            format += $"{f.Key}: {f.Value}\n";
        }
        return format;
    }
    public override string ToString() => $"""
            {Number} / {OriginCountry} ({OriginAirport}) - {DestinationCountry} ({DestinationAirport}) / {DepartureDate} - {ArrivalDate}
            """;
}
