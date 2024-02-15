using AirportBooking.Constants;
using AirportBooking.DTOs;
using AirportBooking.Exceptions;
using AirportBooking.FileReaders;
using AirportBooking.Models;
using AirportBooking.Serializers.Csv;

namespace AirportBooking.Repositories;

public class FlightRepository : IFlightRepository
{
    private List<Flight> _flights = [];
    private readonly CsvFileReader _reader = new();
    private readonly FlightCsvSerializer _serializer = new();
    private readonly static string flightsFileName = DataDirectory.GetFlightsPath();

    public FlightRepository()
    {
        try
        {
            Load();
        }
        catch (Exception ex) when (ex is SerializationException)
        {
            _flights.Clear();
            Console.WriteLine("Couldn't load flight data");
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void Load(string? file = null)
    {
        var readFlights = _reader.Read(file ?? flightsFileName).ToList();
        readFlights.ForEach(line => _flights.Add(_serializer.FromCsv(line)));
    }

    public IReadOnlyList<Flight> FindAll()
    {
        return _flights;
    }

    public Flight Find(string flightNumber)
    {
        return _flights.Find(f => f.Number.Equals(flightNumber))
            ?? throw new EntityNotFound<Flight, string>(flightNumber);
    }

    public IReadOnlyList<Flight> Filter(FlightSearchParameters filters)
    {
        return [];
    }
}
