using AirportBooking.Enums;
using AirportBooking.Models;
using AirportBooking.Validators.CsvValidators;
using AirportBooking.Validators.EntityValidators;

namespace AirportBooking.Serializers.CSVSerializers;

public class FlightCsvSerializer : ICSVSerializer<Flight>
{
    private readonly FlightCsvValidator _csvValidator = new();
    private readonly FlightValidator _entityValidator = new();
    public Flight FromCsv(string csvLine)
    {
        var data = _csvValidator.Validate(csvLine);
        var flightPrices = GetFlightPrices(data);
        var flightNumber = data[0];
        var totalFlightPrices = flightPrices.Count;
        var (originCountry, destinationCountry) = (data[totalFlightPrices + 1], data[totalFlightPrices + 2]);
        var (departureDate, arrivalDate) = (DateTime.Parse(data[totalFlightPrices + 3]),
            DateTime.Parse(data[totalFlightPrices + 4]));
        var (originAirport, destinationAirport) = (data[totalFlightPrices + 5], data[totalFlightPrices + 6]);
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

    private static SortedDictionary<FlightClass, float> GetFlightPrices(string[] data)
    {
        var flightPricesData = data.TakeWhile(data => data.Split(":").Length == 2).ToList();
        var flightPrices = new SortedDictionary<FlightClass, float>();
        flightPricesData.ForEach(price => flightPrices.Add(Enum.Parse<FlightClass>(price.Split(":")[0], true),
               float.Parse(price.Split(":")[1])));
        return flightPrices;
    }

    public string ToCsv(Flight flight)
    {
        _entityValidator.Validate(flight);
        string prices = string.Join(",", flight.ClassPrices.Select(price => $"{price.Key}:{price.Value}"));
        return string.Join(",", [
            flight.Number,
            prices,
            flight.OriginCountry,
            flight.DestinationCountry,
            flight.DepartureDate,
            flight.ArrivalDate,
            flight.OriginAirport,
            flight.DestinationAirport]);
    }
}
