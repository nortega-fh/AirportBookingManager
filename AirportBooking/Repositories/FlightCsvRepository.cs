using AirportBooking.Exceptions;
using AirportBooking.FileReaders;
using AirportBooking.Models;
using AirportBooking.Serializers.Csv;

namespace AirportBooking.Repositories;

public class FlightCsvRepository : IFlightCsvRepository
{
    private readonly ICsvFileReader _reader;
    private readonly IFlightCsvSerializer _serializer;
    private readonly static string FlightsFileName = Path.Combine("..", "..", "..", "Data", "flights.csv");
    private readonly List<string> FilesToLoad = [];

    public FlightCsvRepository(ICsvFileReader reader, IFlightCsvSerializer serializer)
    {
        _reader = reader;
        _serializer = serializer;
    }

    public IReadOnlyList<Flight> FindAll()
    {
        return _reader.Read(FlightsFileName)
            .Concat(FilesToLoad.SelectMany(_reader.Read).ToArray())
            .Select(_serializer.FromCsv)
            .ToList();
    }

    public void AddFileToLoad(string fileName)
    {
        try
        {
            _reader.Read(fileName).ToList().ForEach(line => _serializer.FromCsv(line));
            FilesToLoad.Add(fileName);
        }
        catch (SerializationException)
        {
            throw;
        }
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
