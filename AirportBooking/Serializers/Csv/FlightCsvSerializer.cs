using AirportBooking.Models;
using AirportBooking.Validators;

namespace AirportBooking.Serializers.Csv;

public class FlightCsvSerializer : IFlightCsvSerializer
{
    private readonly IFlightCsvValidator _csvValidator;

    public FlightCsvSerializer(IFlightCsvValidator csvValidator)
    {
        _csvValidator = csvValidator;
    }

    public Flight FromCsv(string csvLine)
    {
        var data = _csvValidator.Validate(csvLine);
        var flightNumber = data[0];
        var flightPrices = GetFlightPrices(data);
        var (originCountry, destinationCountry) = (data[4], data[5]);
        var (departureDate, arrivalDate) = (DateTime.Parse(data[6]), DateTime.Parse(data[7]));
        var (originAirport, destinationAirport) = (data[8], data[9]);
        return new Flight
        {
            Number = flightNumber,
            ClassPrices = flightPrices,
            OriginCountry = originCountry,
            DestinationCountry = destinationCountry,
            DepartureDate = departureDate,
            ArrivalDate = arrivalDate,
            OriginAirport = originAirport,
            DestinationAirport = destinationAirport
        };

    }

    private static SortedDictionary<FlightClass, decimal> GetFlightPrices(string[] data)
    {
        var flightPricesData = data[1..4].ToArray();
        var flightPrices = new SortedDictionary<FlightClass, decimal>();
        for (var i = 0; i < flightPricesData.Length; i++)
        {
            if (flightPricesData[i] is not "null")
            {
                flightPrices.Add((FlightClass)i, decimal.Parse(flightPricesData[i]));
            }
        }
        return flightPrices;
    }
}
