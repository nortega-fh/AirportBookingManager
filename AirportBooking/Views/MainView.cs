using AirportBooking.Exceptions;
using AirportBooking.Models;
using AirportBooking.Repositories;
using AirportBooking.Serializers.ConsoleSerializers;

namespace AirportBooking.Views;

public class MainView
{
    private readonly IUserCsvRepository _userRepository;
    private readonly IFlightCsvRepository _flightRepository;
    private readonly IFlightConsoleSerializer _flightConsoleSerializer;
    private User? _user;

    public MainView(IUserCsvRepository userRepository,
        IFlightCsvRepository flightRepository,
        IFlightConsoleSerializer flightConsoleSerializer)
    {
        _userRepository = userRepository;
        _flightRepository = flightRepository;
        _flightConsoleSerializer = flightConsoleSerializer;
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine("""
            Aiport Booking Manager. Type the number of the option:
            1. Login
            2. Register
            3. Exit
            """);
            string? input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Login();
                    break;
                case "2":
                    Register();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid input, please try again");
                    break;
            }
            if (_user is not null)
            {
                if (_user.Role is UserRole.Manager)
                {
                    ShowManagerMenu();
                }
                else
                {
                    ShowPassengerMenu();
                }
            }
        }
    }

    private void Login()
    {
        Console.WriteLine("Please type your username");
        string username = Console.ReadLine() ?? "";
        Console.WriteLine("Please type your password");
        string password = Console.ReadLine() ?? "";
        var user = _userRepository.Find(username, password);
        if (user is null)
        {
            Console.WriteLine("Login failed. Username or password incorrect");
            return;
        }
        Console.WriteLine("Login successful");
        _user = user;
    }

    private void Register()
    {
        var username = GetNotEmptyString("username");
        var password = GetNotEmptyString("password");

        try
        {
            _user = _userRepository.Create(new User { Username = username, Password = password, Role = UserRole.Passenger });
            Console.WriteLine("User registered succesfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error registering user:");
            Console.WriteLine(ex.Message);
        }
    }

    private string GetNotEmptyString(string property)
    {
        var input = "";
        while (input is "")
        {
            Console.WriteLine($"Please type your {property}");
            input = Console.ReadLine() ?? "";
            if (input is "") Console.WriteLine($"You have to type a valid {property}");
        }
        return input;
    }

    private void ShowPassengerMenu()
    {
        while (true)
        {
            Console.WriteLine("""
            Passenger Menu
            1. Search flights
            2. Go back
            """);
            string? answer = Console.ReadLine();
            switch (answer)
            {
                case "1":
                    SearchFlights();
                    break;
                case "2":
                    return;
                default:
                    Console.WriteLine("Invalid input, please try again");
                    break;
            }
        }
    }

    private void SearchFlights()
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
                    _flightRepository.Filter([.. filters]).ToList().ForEach(_flightConsoleSerializer.PrintToConsole);
                    break;
                case "10":
                    isApplyingFilters = false;
                    break;
            }
        }
    }

    private void GetPriceFilter(List<Predicate<Flight>> filters, bool isMin)
    {
        var priceProp = isMin ? "minimum price" : "maximum price";
        Console.WriteLine($"Please type the {priceProp} you want to look for");
        var typedPrice = GetNotEmptyString(priceProp);
        if (decimal.TryParse(typedPrice, out var price))
        {
            Predicate<Flight> filter = isMin
                ? (Flight flight) => flight.ClassPrices.Values.Min() >= price
                : (Flight flight) => flight.ClassPrices.Values.Max() <= price;
            filters.Add(filter);
        }
        else
        {
            Console.WriteLine("Could not process the number typed, please try again");
        }
    }

    private void GetStringFilter(List<Predicate<Flight>> filters, string filterName, Func<Flight, string> flightProp)
    {
        Console.WriteLine($"Please type the {filterName} that you want to look for");
        var parameter = GetNotEmptyString(filterName);
        filters.Add((Flight flight) => flightProp.Invoke(flight).Equals(parameter, StringComparison.OrdinalIgnoreCase));
    }

    private void GetDepartureDateFilter(List<Predicate<Flight>> filters)
    {
        Console.WriteLine("""
            Please type the departure date of the flight, should be in YYYY-MM-DD format where:
            Y - Year number
            M - Month number
            D - Day of the month number
            """);
        var typedDate = GetNotEmptyString("departure date");
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
        Console.WriteLine("""
            Please select one class to look for the flight:
            1. Economy
            2. Business
            3. First class
            """);
        var typedClass = GetNotEmptyString("flight class");
        switch (typedClass)
        {
            case "1":
                filters.Add((flight) => flight.ClassPrices.Keys.Contains(FlightClass.Economy));
                break;
            case "2":
                filters.Add((flight) => flight.ClassPrices.Keys.Contains(FlightClass.Business));
                break;
            case "3":
                filters.Add((flight) => flight.ClassPrices.Keys.Contains(FlightClass.FirstClass));
                break;
            default:
                Console.WriteLine("Could not recognize selected class. Please try again");
                break;
        }
    }

    private void ShowManagerMenu()
    {
        while (true)
        {
            Console.WriteLine("""
                Manager Menu
                1. Batch Flight Load
                2. Go Back.
                """);
            string? answer = Console.ReadLine();
            switch (answer)
            {
                case "1":
                    RequestFlightsFileName();
                    break;
                case "2":
                    return;
                default:
                    Console.WriteLine("Invalid input, please try again");
                    break;
            }
        }
    }

    private void RequestFlightsFileName()
    {
        Console.WriteLine("Please type the name of the csv file inside the \"Data\" directory from where you want to load the flights:");
        string? fileName = Console.ReadLine();
        if (fileName is null or "")
        {
            Console.WriteLine("Invalid input, please try again");
            return;
        }
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
}