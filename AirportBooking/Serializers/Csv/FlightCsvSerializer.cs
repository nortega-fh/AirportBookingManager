using AirportBooking.Constants;
using AirportBooking.Enums;
using AirportBooking.Models;
using AirportBooking.Validators.CsvValidators;

namespace AirportBooking.Serializers.Csv;

public class FlightCsvSerializer
{
    private readonly FlightCsvValidator _csvValidator = new();
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
        var flightPricesData = data[1..3].ToArray();
        var flightPrices = new SortedDictionary<FlightClass, decimal>();
        for (var i = 0; i < flightPricesData.Length; i++)
        {
            if (data[i] is not CsvValueSkipper.ValueSkipper)
            {
                flightPrices.Add((FlightClass)i, decimal.Parse(data[i]));
            }
        }
        return flightPrices;
    }
}
