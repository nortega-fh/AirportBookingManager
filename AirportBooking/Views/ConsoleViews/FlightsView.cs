using AirportBooking.DTOs;
using AirportBooking.Enums;
using AirportBooking.Exceptions;
using AirportBooking.Models;
using AirportBooking.Repositories;
using AirportBooking.Serializers;

namespace AirportBooking.Views.ConsoleViews;

public class FlightsView : ConsoleViewBase, IQueryableView<Flight, FlightSearchParameters>
{
    private readonly IFileQueryableRepository<string, Flight, FlightSearchParameters> _repository;
    private readonly IFlightConsoleSerializer _serializer;

    public FlightsView(IFileQueryableRepository<string, Flight, FlightSearchParameters> repository,
        IFlightConsoleSerializer serializer)
    {
        _repository = repository;
        _serializer = serializer;
    }

    public Flight? SearchByParameters(FlightSearchParameters parameters)
    {
        var resultFlights = _repository.FindBySearchParameters(parameters);
        if (resultFlights.Count == 0)
        {
            Console.WriteLine("No results match your query parameters");
            return null;
        }
        foreach (var flight in resultFlights)
        {
            _serializer.PrintToConsole(flight);
        }
        var flightNumberList = resultFlights.Select(f => f.Number).ToList();
        return resultFlights.Where(f => f.Number.Equals(GetValue("Please select one of the flights in the list",
            flightNumberList))).First();
    }

    public void ShowAll()
    {
        foreach (var flight in _repository.FindAll())
            _serializer.PrintToConsole(flight);
    }

    public void ShowOne()
    {
        var flightNumber = GetValue("Please indicate the flight's number:");
        try
        {
            _repository.Find(flightNumber);
        }
        catch (Exception ex) when (ex is EntityNotFound<Flight, string>)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            ClearOnInput();
        }
    }

    public FlightSearchParameters ObtainParameters()
    {
        var originCountry = GetValue("Please indicate the origin country");
        var destinationCountry = GetValue("Please indicate the origin country");
        var departureDate = DateTime.Parse(GetValue("Please indicate the flight's departure date as YYYY-MM-DD"));
        var departureAirport = GetOptionalValue("(Optional) Please indicate the flight's departure airport");
        var arrivalAirport = GetOptionalValue("(Optional) Please indicate the flight's arrival airport");
        FlightClass? flightClass = GetOptionalValue("(Optional) Please indicate the preferred class for the flight",
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

    public void Delete()
    {
        var flightNumber = GetValue("Please indicate the flight number to delete:");
        try
        {
            _repository.Delete(flightNumber);
        }
        catch (Exception ex) when (ex is EntityNotFound<Flight, string>)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            ClearOnInput();
        }
    }
}
