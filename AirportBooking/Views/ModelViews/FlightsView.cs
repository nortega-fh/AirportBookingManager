using AirportBooking.DTOs;
using AirportBooking.Enums;
using AirportBooking.Exceptions;
using AirportBooking.Models;
using AirportBooking.Repositories;
using AirportBooking.Serializers;

namespace AirportBooking.Views.ModelViews;

public class FlightsView : ViewInputHandler, IQueryableView<Flight, FlightSearchParameters>
{
    private readonly IFileRepository<string, Flight> _repository;
    private readonly IFlightConsoleSerializer _serializer;

    public FlightsView(IFileRepository<string, Flight> repository, IFlightConsoleSerializer serializer)
    {
        _repository = repository;
        _serializer = serializer;
    }

    public FlightSearchParameters GetFlightFilters(BookingType bookingType = BookingType.OneWay)
    {
        Console.WriteLine("Please write the country of origin:");
        var originCountry = Console.ReadLine() ?? "";
        while (originCountry.Equals(string.Empty))
        {
            Console.WriteLine("Invalid input. Please try again.");
            originCountry = Console.ReadLine() ?? "";
        }

        Console.WriteLine("Please write the destination country:");
        var destinationCountry = Console.ReadLine() ?? "";
        while (destinationCountry.Equals(string.Empty))
        {
            Console.WriteLine("Invalid input. Please try again.");
            destinationCountry = Console.ReadLine() ?? "";
        }

        Console.WriteLine("""
            Please write the departure date in format "YYYY-MM-DD":
            Y - Year number
            M - Month number
            D - Day of month number
            """);
        var date = Console.ReadLine() ?? "";
        while (date.Equals(string.Empty))
        {
            Console.WriteLine("Invalid input. Please try again.");
            date = Console.ReadLine() ?? "";
        }
        var departureDate = DateTime.Parse(date);

        DateTime? returnDate = null;
        if (bookingType == BookingType.RoundTrip)
        {
            Console.WriteLine("""
            Please write the return date in format "YYYY-MM-DD":
            Y - Year number
            M - Month number
            D - Day of month number
            """);
            date = Console.ReadLine() ?? "";
            while (date.Equals(string.Empty))
            {
                Console.WriteLine("Invalid input. Please try again.");
                date = Console.ReadLine() ?? "";
            }
            returnDate = DateTime.Parse(date);
        }

        Console.WriteLine("(Optional) Write a preferred departure airport:");
        var departureAirport = Console.ReadLine();
        if (departureAirport is "") departureAirport = null;

        Console.WriteLine("(Optional) Write a preferred arrival airport:");
        var arrivalAirport = Console.ReadLine();
        if (arrivalAirport is "") arrivalAirport = null;

        Console.WriteLine("""
            (Optional) Select the class that the flight must include in its options: 
            1. Economy
            2. Business
            3. First class
            """);
        FlightClass? flightClass = Console.ReadLine() switch
        {
            "1" => FlightClass.Economy,
            "2" => FlightClass.Business,
            "3" => FlightClass.FirstClass,
            _ => null
        };

        Console.WriteLine("(Optional) Minimum price for the flight: ");
        var price = Console.ReadLine() ?? "";
        var minPrice = 0f;
        if (!price.Equals(string.Empty)) minPrice = float.Parse(price);

        Console.WriteLine("(Optional) Maximum price for the flight: ");
        price = Console.ReadLine() ?? "";
        var maxPrice = float.MaxValue;
        if (!price.Equals(string.Empty)) maxPrice = float.Parse(price);

        return new FlightSearchParameters(originCountry, destinationCountry, departureDate, returnDate,
            departureAirport, arrivalAirport, flightClass, minPrice, maxPrice);
    }

    public Flight? GetFlight()
    {
        Console.WriteLine("Please type the flight number:");
        var flightNumber = Console.ReadLine() ?? "";
        if (flightNumber is "") return null;
        var flight = repository.Find(flightNumber);
        Console.WriteLine(flight is not null ? flight : $"Flight with number {flightNumber} not found");
        return flight;
    }

    public FlightClass ChooseFlightPrice(Flight flight)
    {
        Console.WriteLine(flight.PricesToString());
        Console.WriteLine("""
            Please choose the class with which you want to flight (Default Economy):
            1. Economy
            2. Business
            3. First Class
            """);
        return Console.ReadLine() switch
        {
            "" or "1" => FlightClass.Economy,
            "2" => FlightClass.Business,
            "3" => FlightClass.FirstClass,
            _ => FlightClass.Economy
        };
    }

    public void ShowFlights(FlightSearchParameters? parameters = null)
    {
        Console.Clear();
        var flights = parameters is null ? repository.FindAll() : repository.FindBySearchParameters(parameters);
        flights.ToList().ForEach(Console.WriteLine);
    }

    public void SearchByParameters(FlightSearchParameters parameters)
    {
        throw new NotImplementedException();
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

    public void Create()
    {

    }

    public void Update()
    {
        throw new NotImplementedException();
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
