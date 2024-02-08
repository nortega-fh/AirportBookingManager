using AirportBooking.DTOs;
using AirportBooking.Exceptions;
using AirportBooking.FileReaders;
using AirportBooking.Models;
using AirportBooking.Serializers.CSVSerializers;
using AirportBooking.Validators.EntityValidators;
using System.Collections.Immutable;

namespace AirportBooking.Repositories;

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
        catch (Exception ex) when (ex is ArgumentException || ex is InvalidAttributeException)
        {
            _flights.Clear();
            Console.WriteLine("Couldn't load flight data");
            Console.WriteLine(ex.Message);
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

    public IReadOnlyList<Flight> FindBySearchParameters(FlightSearchParameters parameters)
    {
        IEnumerable<Flight> resultFlights = _flights
            .Where(f => f.OriginCountry.Contains(parameters.OriginCountry, StringComparison.OrdinalIgnoreCase))
            .Where(f => f.DestinationCountry.Contains(parameters.DestinationCountry, StringComparison.OrdinalIgnoreCase))
            .Where(f => parameters.DepartureDate <= f.DepartureDate && f.DepartureDate < parameters.DepartureDate.AddDays(1)
            && f.DepartureDate > parameters.DepartureDate.AddDays(-1))
            .Where(f => f.ClassPrices.Values.Min() >= parameters.MinPrice && f.ClassPrices.Values.Min() <= parameters.MaxPrice);
        if (parameters.DepartureAirport is (not null) and (not ""))
        {
            resultFlights = resultFlights.Where(f => f.OriginAirport.Equals(parameters.DepartureAirport,
                StringComparison.OrdinalIgnoreCase));
        }
        if (parameters.ArrivalAirport is (not null) and (not ""))
        {
            resultFlights = resultFlights.Where(f => f.DestinationAirport.Equals(parameters.ArrivalAirport,
                StringComparison.OrdinalIgnoreCase));
        }
        if (parameters.FlightClass is not null)
        {
            resultFlights = resultFlights.Where(f => f.ClassPrices.ContainsKey(parameters.FlightClass.Value));
        }
        return resultFlights.ToImmutableList();
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
