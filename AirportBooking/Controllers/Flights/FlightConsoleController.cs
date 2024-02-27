using AirportBooking.ConsoleInputHandler;
using AirportBooking.Exceptions;
using AirportBooking.Models.Bookings;
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

    public IReadOnlyList<Flight> SearchFlightsToBook(BookingType bookingType)
    {
        var flights = new List<Flight>();
        var totalFlightsToBook = bookingType == BookingType.OneWay ? 1 : 2;
        for (var i = 0; i < totalFlightsToBook; i++)
        {
            flights.Add(GetFlightToBook());
        }
        return flights;
    }

    public Flight GetFlightToBook()
    {
        var filters = new List<Predicate<Flight>>();
        GetStringFilter(filters, "departure country", flight => flight.OriginCountry);
        GetStringFilter(filters, "arrival country", flight => flight.DestinationCountry);
        GetDepartureDateFilter(filters);
        Console.WriteLine("""
            Do you want to add more filters to your search?
            1. Yes
            2. No
            """);
        Console.Clear();
        var selectedFlight = new Flight();
        switch (Console.ReadLine())
        {
            case "1":
                AddOptionalParams(filters);
                goto case "2";
            case "2":
                selectedFlight = SelectFlightFromList(GetFlightsWithParams([.. filters]));
                filters.Clear();
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
        return selectedFlight;
    }

    private void AddOptionalParams(IList<Predicate<Flight>> filters)
    {
        while (true)
        {
            Console.WriteLine("""
            Add additional filters for the flight:
            1. Select minimum price.
            2. Select maximum price.
            3. Select departure airport.
            4. Select arrival airport.
            5. Select flight class.
            6. Apply filters
            """);
            switch (Console.ReadLine())
            {
                case "1":
                    GetPriceFilter(filters, true);
                    break;
                case "2":
                    GetPriceFilter(filters, false);
                    break;
                case "3":
                    GetStringFilter(filters, "departure airport", flight => flight.OriginAirport);
                    break;
                case "4":
                    GetStringFilter(filters, "arrival airport", flight => flight.DestinationAirport);
                    break;
                case "5":
                    GetFlightClassFilter(filters);
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid input, please try again");
                    break;
            }
        }
    }

    private IEnumerable<Flight> GetFlightsWithParams(params Predicate<Flight>[] filters)
    {
        return _flightRepository.Filter(filters);
    }

    private Flight SelectFlightFromList(IEnumerable<Flight> flights)
    {
        PrintFlightList(flights);
        var flightNumber = _consoleInputHandler.GetNotEmptyString("Please type the flight number that you want to select");
        var selectedFlight = flights.Where(f => f.Number.Equals(flightNumber)).FirstOrDefault();
        while (selectedFlight is null)
        {
            Console.WriteLine($"The flight with flight number {flightNumber} is not in the previous list. Please try again");
            selectedFlight = flights.Where(f => f.Number.Equals(flightNumber)).FirstOrDefault();
        }
        return selectedFlight;
    }

    private void PrintFlightList(IEnumerable<Flight> flights)
    {
        flights.ToList().ForEach(_flightConsoleSerializer.PrintToConsoleWithPrices);
    }

    public IReadOnlyList<Flight> SearchFlights()
    {
        var filters = new List<Predicate<Flight>>();
        var flights = new List<Flight>();
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
                    flights = GetFlightsWithParams([.. filters]).ToList();
                    PrintFlightList(flights);
                    filters.Clear();
                    break;
                case "10":
                    isApplyingFilters = false;
                    break;
            }
        }
        return flights;
    }

    private void GetPriceFilter(IList<Predicate<Flight>> filters, bool isMin)
    {
        var priceProp = isMin ? "minimum price" : "maximum price";
        Console.WriteLine($"Please type the {priceProp} you want to look for");
        var price = _consoleInputHandler.GetDecimal();
        Predicate<Flight> filter = isMin
            ? (Flight flight) => flight.ClassPrices.Values.Min() >= price
            : (Flight flight) => flight.ClassPrices.Values.Max() <= price;
        filters.Add(filter);
    }

    private void GetStringFilter(IList<Predicate<Flight>> filters, string filterName, Func<Flight, string> flightProp)
    {
        var parameter = _consoleInputHandler.GetNotEmptyString($"Please type the {filterName} that you want to look for");
        filters.Add((flight) => flightProp.Invoke(flight).Contains(parameter, StringComparison.OrdinalIgnoreCase));
    }

    private void GetDepartureDateFilter(IList<Predicate<Flight>> filters)
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

    private void GetFlightClassFilter(IList<Predicate<Flight>> filters)
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

    public FlightClass GetClassForFlight(Flight flight)
    {
        Console.WriteLine($"Please select the new class for the flight {flight.Number}:");
        ShowFlightClasses(flight);
        var option = _consoleInputHandler.GetInteger();
        while (option < 1 || option > flight.ClassPrices.Count)
        {
            Console.WriteLine($"Option provided \"{option}\" is not within the options range. Please try again.");
            option = _consoleInputHandler.GetInteger();
        }
        return flight.ClassPrices.Keys.ElementAt(option - 1);
    }
}
