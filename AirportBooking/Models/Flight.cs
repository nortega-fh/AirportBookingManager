using AirportBooking.Enums;

namespace AirportBooking.Models
{
    public class Flight(string number, SortedDictionary<FlightClass, float> prices, string originCountry, string destinationCountry,
        DateTime departureDate, DateTime arrivalDate, string originAirport, string destinationAirport)
    {
        public string Number { get; set; } = number;
        public string OriginCountry { get; set; } = originCountry;
        public string DestinationCountry { get; set; } = destinationCountry;
        public DateTime DepartureDate { get; set; } = departureDate;
        public DateTime ArrivalDate { get; set; } = arrivalDate;
        public string OriginAirport { get; set; } = originAirport;
        public string DestinationAirport { get; set; } = destinationAirport;
        public SortedDictionary<FlightClass, float> ClassPrices { get; set; } = prices;

        public string ToCSV() => string.Join(",", [Number, .. ClassPrices.Values, OriginCountry, DestinationCountry, DepartureDate, ArrivalDate, OriginAirport, DestinationAirport]);

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
}
