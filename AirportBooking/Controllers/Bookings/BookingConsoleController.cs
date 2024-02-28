using AirportBooking.ConsoleInputHandler;
using AirportBooking.Controllers.Flights;
using AirportBooking.Models.Bookings;
using AirportBooking.Models.Flights;
using AirportBooking.Models.Users;
using AirportBooking.Repositories.Bookings;
using AirportBooking.Serializers.Console.Bookings;

namespace AirportBooking.Controllers.Bookings;

public class BookingConsoleController : IBookingConsoleController
{
    private readonly IConsoleInputHandler _consoleInputHandler;
    private readonly IBookingCsvRepository _repository;
    private readonly IBookingConsoleSerializer _serializer;
    private readonly IFlightConsoleController _flightController;

    public BookingConsoleController(
        IConsoleInputHandler consoleInputHandler,
        IBookingCsvRepository repository,
        IBookingConsoleSerializer serializer,
        IFlightConsoleController flightController)
    {
        _consoleInputHandler = consoleInputHandler;
        _repository = repository;
        _serializer = serializer;
        _flightController = flightController;
    }

    public void ShowUserBookings(User user)
    {
        _repository.Filter(booking => booking.MainPassenger?.Username == user.Username).ToList().ForEach(_serializer.PrintToConsole);
    }

    public void SearchBookings()
    {
        var searchParams = new List<Predicate<Booking>>();
        while (true)
        {
            Console.WriteLine("""
            Search Bookings
            Here you can add a filter with each of the options available, once you have included all the desired filters you
            can type "11" to make the search. Filters will be applied in the order you added them.
            1. Search for Minimum price.
            2. Search for Maximum price.
            3. Search for Flight Number.
            4. Search for Departure country.
            5. Search for Destination country.
            6. Search for Departure date.
            7. Search for Departure airport.
            8. Search for Arrival airport.
            9. Search for Passenger
            10. Search for Flight Class.
            11. Search booking
            12. Exit
            """);
            switch (Console.ReadLine())
            {
                case "1":
                    searchParams.Add(GetPriceFilter());
                    break;
                case "2":
                    searchParams.Add(GetPriceFilter(false));
                    break;
                case "3":
                    var flightNumber = _consoleInputHandler.GetNotEmptyString("Please type the flight number to look for");
                    searchParams.Add(booking => booking.Flights.Any(flight => flight.Number == flightNumber));
                    break;
                case "4":
                    searchParams.Add(GetFlightValueContainsFilter("Please type the departure country to look for", booking => booking.Flights.First(), flight => flight.OriginCountry));
                    break;
                case "5":
                    searchParams.Add(GetFlightValueContainsFilter("Please type the destination country to look for", booking => booking.Flights.Last(), flight => flight.DestinationCountry));
                    break;
                case "6":
                    searchParams.Add(GetBookingDepartureDateFilter());
                    break;
                case "7":
                    searchParams.Add(GetFlightValueContainsFilter("Please type the departure airport to look for", booking => booking.Flights.First(), flight => flight.OriginAirport));
                    break;
                case "8":
                    searchParams.Add(GetFlightValueContainsFilter("Please type the arrival airport to look for", booking => booking.Flights.Last(), flight => flight.DestinationAirport));
                    break;
                case "9":
                    var username = _consoleInputHandler.GetNotEmptyString("Please type the user's username to look for");
                    searchParams.Add(booking => booking.MainPassenger is not null && booking.MainPassenger.Username.Contains(username, StringComparison.OrdinalIgnoreCase));
                    break;
                case "10":
                    searchParams.Add(GetFlightClassFilter());
                    break;
                case "11":
                    _repository.Filter([.. searchParams]).ToList().ForEach(_serializer.PrintToConsole);
                    searchParams.Clear();
                    return;
                case "12":
                    return;
                default:
                    Console.WriteLine("Option invalid. Please try again");
                    break;
            }
        }
    }

    private Predicate<Booking> GetPriceFilter(bool isMin = true)
    {
        Console.WriteLine("Please type the price to filter for the booking");
        var price = _consoleInputHandler.GetDecimal();
        if (isMin)
        {
            return booking => booking.CalculatePrice() >= price;
        }
        return booking => booking.CalculatePrice() <= price;
    }

    private Predicate<Booking> GetFlightValueContainsFilter(string message, Func<Booking, Flight> bookingFlight, Func<Flight, string> flightValue)
    {
        var value = _consoleInputHandler.GetNotEmptyString(message);
        return booking => flightValue.Invoke(bookingFlight.Invoke(booking)).Contains(value, StringComparison.OrdinalIgnoreCase);
    }

    private Predicate<Booking> GetBookingDepartureDateFilter()
    {
        var departureDate = _consoleInputHandler.GetNotEmptyString("""
            Please type the booking's departure date to look for in format YYYY-MM-DD
            Y - Year number
            M - Month number
            D - Day of month number
            """);
        var isDateValid = DateTime.TryParse(departureDate, out var date);
        while (!isDateValid)
        {
            Console.WriteLine("Could not read date, please try again.");
            isDateValid = DateTime.TryParse(departureDate, out date);
        }
        return booking => booking.Flights.First().DepartureDate >= date.AddDays(-1) && booking.Flights.First().DepartureDate <= date.AddDays(1);
    }

    private Predicate<Booking> GetFlightClassFilter()
    {
        var classFilter = _flightController.GetFlightClassFilter();
        return booking => booking.Flights.Find(flight => classFilter.Invoke(flight)) is not null;
    }

    public void CancelBooking()
    {
        var booking = GetBooking();
        if (booking.Status == BookingStatus.Canceled)
        {
            Console.WriteLine($"Booking with reservation number \"{booking.ReservationNumber}\" is already canceled");
            return;
        }
        Console.WriteLine("Are you sure you want to cancel the following booking?");
        _serializer.PrintToConsole(booking);
        Console.WriteLine("""
            1. Yes
            2. No
            """);
        switch (Console.ReadLine())
        {
            case "1":
                booking.Status = BookingStatus.Canceled;
                _repository.Update(booking.ReservationNumber, booking);
                Console.WriteLine("Booking succesfully canceled");
                break;
            case "2":
                break;
            default:
                Console.WriteLine("Option invalid. Please try again");
                break;
        }
    }

    private Booking GetBooking()
    {
        Console.WriteLine("Please type the reservation number of the booking:");
        var reservationNumber = _consoleInputHandler.GetInteger();
        var booking = _repository.Find(reservationNumber);
        while (booking is null)
        {
            Console.WriteLine($"The booking with reservation number {reservationNumber} doesn't exists");
            reservationNumber = _consoleInputHandler.GetInteger();
            booking = _repository.Find(reservationNumber);
        }
        return booking;
    }

    public void CreateBooking(User user)
    {
        var booking = new Booking { MainPassenger = user };
        SetBookingType(booking);
        SetBookingFlights(booking);
        _serializer.PrintToConsole(booking);
        Console.WriteLine("""
        Do you want to confirm this booking?
        1. Yes
        2. No
        """);
        switch (Console.ReadLine())
        {
            case "1":
                _repository.Save(booking);
                break;
            case "2":
                break;
            default:
                Console.WriteLine("Invalid input. Please try again");
                break;
        }
    }

    private void SetBookingType(Booking booking)
    {
        Console.WriteLine("Please choose the type of the booking");
        _serializer.ShowBookingTypes();
        var bookingTypeList = Enum.GetNames<BookingType>();
        var option = _consoleInputHandler.GetInteger();
        if (option < 0 || option > bookingTypeList.Length)
        {
            Console.WriteLine("Invald option, please try again");
            return;
        }
        var chosenType = Enum.Parse<BookingType>(bookingTypeList[option - 1]);
        booking.BookingType = chosenType;
    }

    private void SetBookingFlights(Booking booking)
    {
        var numberOfFlightsToAdd = booking.BookingType == BookingType.OneWay ? 1 : 2;
        var bookingModifiedFlights = new List<Flight>();
        for (var i = 0; i < numberOfFlightsToAdd; i++)
        {
            bookingModifiedFlights.Add(_flightController.GetFlightToBook());
        }
        if (bookingModifiedFlights.First().DepartureDate > bookingModifiedFlights.Last().DepartureDate)
        {
            Console.WriteLine("Error: last flight's departure date can't be before than first flight's departure date");
            return;
        }
        booking.Flights = bookingModifiedFlights;
        SetBookingClasses(booking);
    }

    private void SetBookingClasses(Booking booking)
    {
        var modifiedClasses = new List<FlightClass>();
        booking.Flights.ForEach(flight => modifiedClasses.Add(_flightController.GetClassForFlight(flight)));
        booking.FlightClasses = modifiedClasses;
        Console.Clear();
    }

    public void UpdateBooking(User user)
    {
        ShowUserBookings(user);
        var booking = GetBooking();
        _serializer.PrintToConsole(booking);
        var isEditing = true;
        while (isEditing)
        {
            Console.WriteLine("""
            Select the changes you want to make to the booking:
            1. Change flights
            2. Change class
            3. Confirm
            """);
            switch (Console.ReadLine())
            {
                case "1":
                    SetBookingFlights(booking);
                    break;
                case "2":
                    SetBookingClasses(booking);
                    break;
                case "3":
                    isEditing = false;
                    break;
                default:
                    Console.WriteLine("Option not valid. Please try again");
                    break;
            }
        }
        Console.Clear();
        _repository.Update(booking.ReservationNumber, booking);
    }
}