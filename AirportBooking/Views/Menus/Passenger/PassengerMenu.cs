using AirportBooking.Controllers.Bookings;
using AirportBooking.Controllers.Flights;
using AirportBooking.Views.Context;

namespace AirportBooking.Views.Menus.Passenger;

public class PassengerMenu : IPassengerMenu
{
    private readonly IFlightConsoleController _flightConsoleController;
    private readonly IBookingConsoleController _bookingConsoleController;
    private readonly IUserSession _userSession;

    public PassengerMenu(
        IUserSession userSession,
        IFlightConsoleController flightConsoleController,
        IBookingConsoleController bookingConsoleController)
    {
        _userSession = userSession;
        _flightConsoleController = flightConsoleController;
        _bookingConsoleController = bookingConsoleController;
    }

    public void ShowPassengerMenu()
    {
        while (_userSession.IsLoggedIn())
        {
            Console.WriteLine("""
            Passenger Menu
            1. Bookings
            2. Search flights
            3. Logout
            """);
            string? answer = Console.ReadLine();
            Console.Clear();
            switch (answer)
            {
                case "1":
                    ManageBookings();
                    break;
                case "2":
                    _flightConsoleController.SearchFlights();
                    break;
                case "3":
                    _userSession.LogOutUser();
                    return;
                default:
                    Console.WriteLine("Invalid input, please try again");
                    break;
            }
        }
    }

    private void ManageBookings()
    {
        while (true)
        {
            Console.WriteLine("""
                Bookings
                1. Book a flight
                2. Manage personal bookings
                3. Go back.
                """);
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    BookFlight();
                    break;
                case "2":
                    ShowBookingManager();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid input, please try again");
                    break;
            }
        }
    }
    private void BookFlight()
    {
        var currentUser = _userSession.GetUser();
        if (currentUser is null)
        {
            return;
        }
        _bookingConsoleController.CreateBooking(currentUser);
    }

    private void ShowBookingManager()
    {
        var currentUser = _userSession.GetUser();
        if (currentUser is null)
        {
            return;
        }
        while (true)
        {
            Console.WriteLine("""
                Booking manager
                1. Show my bookings
                2. Edit booking
                3. Delete booking
                4. Go back
                """);
            switch (Console.ReadLine())
            {
                case "1":
                    _bookingConsoleController.ShowUserBookings(currentUser);
                    break;
                case "2":
                    _bookingConsoleController.UpdateBooking(currentUser);
                    break;
                case "3":
                    _bookingConsoleController.CancelBooking();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid input, please try again");
                    break;
            }
        }
    }
}
