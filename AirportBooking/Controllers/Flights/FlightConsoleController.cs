using AirportBooking.ConsoleInputHandler;
using AirportBooking.Exceptions;
using AirportBooking.Models.Flights;
using AirportBooking.Repositories.Flights;
using AirportBooking.Serializers.Console.Flights;

namespace AirportBooking.Controllers.Flights;

public class FlightConsoleController : IFlightConsoleController
{
    private readonly IFlightCsvRepository _flightRepository;
    private readonly IFlightConsoleSerializer _flightConsoleSerializer;
    private readonly IConsoleInputHandler _consoleInputHandler;

    public FlightConsoleController(
        IFlightCsvRepository repository,
        IFlightConsoleSerializer consoleSerializer,
        IConsoleInputHandler consoleInputHandler)
    {
        _flightRepository = repository;
        _flightConsoleSerializer = consoleSerializer;
        _consoleInputHandler = consoleInputHandler;
    }

    public IReadOnlyList<Flight> SearchFlights()
    {
        var filters = new List<Predicate<Flight>>();
        bool isApplyingFilters = true;
        while (isApplyingFilters)
        {
            Console.WriteLine("""
            Search Flights
            Add the filters by which you want to look your flight typing the number of one of the options below
            Once you are done, type the option 9 to see the result list.
            1. Select minimum price.
            2. Select maximum price.
            3. Select departure country.
            4. Select destination country.
            5. Select departure date.
            6. Select departure airport.
            7. Select arrival airport.
            8. Select flight class.
            9. Search
            10. Exit
            """);
            string? option = Console.ReadLine();
            Console.Clear();
            switch (option)
            {
                case "1":
                    GetPriceFilter(filters, true);
                    break;
                case "2":
                    GetPriceFilter(filters, false);
                    break;
                case "3":
                    GetStringFilter(filters, "departure country", (flight) => flight.OriginCountry);
                    break;
                case "4":
                    GetStringFilter(filters, "destination country", (flight) => flight.DestinationCountry);
                    break;
                case "5":
                    GetDepartureDateFilter(filters);
                    break;
                case "6":
                    GetStringFilter(filters, "departure airport", (flight) => flight.OriginAirport);
                    break;
                case "7":
                    GetStringFilter(filters, "arrival airport", (flight) => flight.DestinationAirport);
                    break;
                case "8":
                    GetFlightClassFilter(filters);
                    break;
                case "9":
                    var flights = _flightRepository.Filter([.. filters]).ToList();
                    flights.ForEach(_flightConsoleSerializer.PrintToConsoleWithPrices);
                    filters.Clear();
                    return flights;
                case "10":
                    isApplyingFilters = false;
                    break;
            }
        }
        return [];
    }

    private void GetPriceFilter(List<Predicate<Flight>> filters, bool isMin)
    {
        var priceProp = isMin ? "minimum price" : "maximum price";
        Console.WriteLine($"Please type the {priceProp} you want to look for");
        var price = _consoleInputHandler.GetDecimal();
        Predicate<Flight> filter = isMin
            ? (Flight flight) => flight.ClassPrices.Values.Min() >= price
            : (Flight flight) => flight.ClassPrices.Values.Max() <= price;
        filters.Add(filter);
    }

    private void GetStringFilter(List<Predicate<Flight>> filters, string filterName, Func<Flight, string> flightProp)
    {
        var parameter = _consoleInputHandler.GetNotEmptyString($"Please type the {filterName} that you want to look for");
        filters.Add((flight) => flightProp.Invoke(flight).Equals(parameter, StringComparison.OrdinalIgnoreCase));
    }

    private void GetDepartureDateFilter(List<Predicate<Flight>> filters)
    {
        var typedDate = _consoleInputHandler.GetNotEmptyString("""
            Please type the departure date of the flight, should be in YYYY-MM-DD format where:
            Y - Year number
            M - Month number
            D - Day of the month number
            """);
        if (DateTime.TryParse(typedDate, out var date))
        {
            filters.Add((flight) => flight.DepartureDate >= date.AddDays(-1) && flight.DepartureDate <= date.AddDays(1));
        }
        else
        {
            Console.WriteLine("Could not process the date typed, please try again");
        }
    }

    private void GetFlightClassFilter(List<Predicate<Flight>> filters)
    {
        var typedClass = _consoleInputHandler.GetNotEmptyString("""
            Please select one class to look for the flight:
            1. Economy
            2. Business
            3. First class
            """);
        Console.Clear();
        switch (typedClass)
        {
            case "1":
                filters.Add((flight) => flight.ClassPrices.ContainsKey(FlightClass.Economy));
                break;
            case "2":
                filters.Add((flight) => flight.ClassPrices.ContainsKey(FlightClass.Business));
                break;
            case "3":
                filters.Add((flight) => flight.ClassPrices.ContainsKey(FlightClass.FirstClass));
                break;
            default:
                Console.WriteLine("Could not recognize selected class. Please try again");
                break;
        }
    }

    public void RequestFlightsFileName()
    {
        var fileName = _consoleInputHandler.GetNotEmptyString("Please type the name of the csv file inside the \"Data\" directory from where you want to load the flights:");
        try
        {
            var path = Path.Combine("..", "..", "..", "Data", fileName.EndsWith(".csv") ? fileName : fileName + ".csv");
            _flightRepository.AddFileToLoad(path);
            Console.WriteLine("Flights load succesfully");
        }
        catch (SerializationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void ShowFlightClasses(Flight flight)
    {
        _flightConsoleSerializer.ShowFlightAvailableClasses(flight);
    }
}
