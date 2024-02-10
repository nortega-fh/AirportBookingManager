using AirportBooking.DTOs;
using AirportBooking.Enums;
using AirportBooking.Exceptions;
using AirportBooking.Filters;
using AirportBooking.Models;
using AirportBooking.Repositories;
using AirportBooking.Serializers;

namespace AirportBooking.Views.Controllers;

public class FlightsController : ConsoleViewBase
{
    private readonly IFileRepository<string, Flight> _repository;
    private readonly IFilter<Flight, FlightSearchParameters> _filter;
    private readonly IFlightConsoleSerializer _serializer;

    public FlightsController(IFileRepository<string, Flight> repository, IFlightConsoleSerializer serializer,
        IFilter<Flight, FlightSearchParameters> filter)
    {
        _repository = repository;
        _serializer = serializer;
        _filter = filter;
    }

    public IReadOnlyList<Flight>? SearchByParameters(FlightSearchParameters parameters)
    {
        var resultFlights = _filter.SearchByParameters(parameters, _repository.FindAll().ToList());
        if (resultFlights.Count == 0)
        {
            return null;
        }
        return resultFlights;
    }

    public IReadOnlyList<Flight> FindAll()
    {
        return _repository.FindAll();
    }

    public Flight? Find(string flightNumber)
    {
        try
        {
            return _repository.Find(flightNumber);
        }
        catch (Exception ex) when (ex is EntityNotFound<Flight, string>)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }

    public FlightSearchParameters ObtainFilterParameters()
    {
        var originCountry = GetValue("Please indicate the origin country");
        var destinationCountry = GetValue("Please indicate the arrival country");
        var departureDate = DateTime.Parse(GetValue("Please indicate the flight's departure date as YYYY-MM-DD"));
        var departureAirport = GetOptionalValue("(Optional) Please indicate the flight's departure airport");
        var arrivalAirport = GetOptionalValue("(Optional) Please indicate the flight's arrival airport");
        FlightClass? flightClass = GetOptionalValue("(Optional) Please indicate the class the flight must include",
            [FlightClass.Economy, FlightClass.Business, FlightClass.FirstClass], out var obtainedClass)
            ? obtainedClass : null;
        var minPrice = GetOptionalFloatValue("(Optional) Please indicate the minimum value to look for");
        var maxPrice = GetOptionalFloatValue("(Optional) Please indicate the maximum value to look for");
        return new FlightSearchParameters(
                originCountry,
                destinationCountry,
                departureDate,
                departureAirport,
                arrivalAirport,
                flightClass,
                minPrice,
                maxPrice
            );
    }

    public void Delete(string flightNumber)
    {
        try
        {
            _repository.Delete(flightNumber);
        }
        catch (Exception ex) when (ex is EntityNotFound<Flight, string>)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public Flight Create(Flight flight)
    {
        return null;
    }

    public void PrintToConsole(Flight flight)
    {
        _serializer.PrintToConsole(flight);
    }
}
