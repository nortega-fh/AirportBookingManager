using AirportBooking.FileReaders;
using AirportBooking.Models;
using AirportBooking.Serializers.Csv;

namespace AirportBooking.Repositories;

public class FlightCsvRepository : IFlightCsvRepository
{
    private readonly IFileReader _reader;
    private readonly IFlightCsvSerializer _serializer;
    private readonly static string FlightsFileName = Path.Combine("..", "..", "..", "Data", "flights.csv");

    public FlightCsvRepository(IFileReader reader, IFlightCsvSerializer serializer)
    {
        _reader = reader;
        _serializer = serializer;
    }

    public IReadOnlyList<Flight> FindAll()
    {
        return _reader.Read(FlightsFileName)
            .Select(_serializer.FromCsv)
            .ToList();
    }

    public Flight? Find(string flightNumber)
    {
        return _reader.Read(FlightsFileName)
            .Select(_serializer.FromCsv)
            .Where(flight => flight.Number == flightNumber)
            .FirstOrDefault();
    }

    public IReadOnlyList<Flight> Filter(params Predicate<Flight>[] filters)
    {
        var flightList = FindAll();
        foreach (var filter in filters)
        {
            flightList = flightList.Where(filter.Invoke).ToList();
        }
        return flightList;
    }
}
