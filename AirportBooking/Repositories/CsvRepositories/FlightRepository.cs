using AirportBooking.Exceptions;
using AirportBooking.FileReaders;
using AirportBooking.Models;
using AirportBooking.Serializers.CSVSerializers;
using AirportBooking.Validators.EntityValidators;

namespace AirportBooking.Repositories.CsvRepositories;

public class FlightRepository : IFileRepository<string, Flight>
{
    private List<Flight> _flights = [];
    private readonly CSVReader _reader = new("flights");
    private readonly FlightCsvSerializer _serializer = new();
    private readonly FlightValidator _validator = new();
    public FlightRepository()
    {
        try
        {
            Load();
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidAttributeException or EntitySerializationException<Flight>)
        {
            _flights.Clear();
            Console.WriteLine("Couldn't load flight data");
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void Load()
    {
        var readFlights = _reader.ReadEntityInformation().ToList();
        readFlights.ForEach(line => _flights.Add(_serializer.FromCsv(line)));
    }

    public Flight Find(string flightNumber)
    {
        return _flights.Find(f => f.Number.Equals(flightNumber, StringComparison.OrdinalIgnoreCase))
            ?? throw new EntityNotFound<Flight, string>(flightNumber);
    }

    public IReadOnlyList<Flight> FindAll()
    {
        return _flights;
    }

    public Flight Save(Flight flight)
    {
        var existingFlight = _flights.Find(f => f.Number.Equals(flight.Number, StringComparison.OrdinalIgnoreCase));
        if (existingFlight is not null)
        {
            throw new EntityAlreadyExists<Flight, string>(flight.Number);
        }
        _validator.Validate(flight);
        _reader.WriteEntityInformation(_serializer.ToCsv(flight));
        _flights.Add(flight);
        return flight;
    }

    public Flight Update(string flightNumber, Flight newFlight)
    {
        if (_flights.Find(f => f.Number.Equals(flightNumber, StringComparison.OrdinalIgnoreCase)) is null)
        {
            throw new EntityNotFound<Flight, string>(flightNumber);
        }
        _validator.Validate(newFlight);
        _reader.UpdateEntityInformation(flightNumber, _serializer.ToCsv(newFlight));
        _flights = _flights.Select(f => f.Number.Equals(flightNumber,
            StringComparison.OrdinalIgnoreCase) ? newFlight : f).ToList();
        return newFlight;
    }

    public void Delete(string flightNumber)
    {
        if (_flights.Find(f => f.Number.Equals(flightNumber, StringComparison.OrdinalIgnoreCase)) is null)
        {
            throw new EntityNotFound<Flight, string>(flightNumber);
        }
        _reader.DeleteEntityInformation(flightNumber);
        _flights = _flights.Where(f => !f.Number.Equals(flightNumber, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}
