using AirportBooking.Controllers.Bookings;
using AirportBooking.Controllers.Flights;
using AirportBooking.Views.Context;

namespace AirportBooking.Views.Menus.Manager;

public class ManagerMenu : IManagerMenu
{
    private readonly IFlightConsoleController _flightConsoleController;
    private readonly IBookingConsoleController _bookingConsoleController;
    private readonly IUserSession _userSession;

    public ManagerMenu(
        IUserSession userSession,
        IFlightConsoleController flightConsoleController,
        IBookingConsoleController bookingConsoleController)
    {
        _userSession = userSession;
        _flightConsoleController = flightConsoleController;
        _bookingConsoleController = bookingConsoleController;
    }

    public void ShowManagerMenu()
    {
        while (_userSession.IsLoggedIn())
        {
            Console.WriteLine("""
                Manager Menu
                1. Search Booking
                2. Batch Flight Load
                3. Logout.
                """);
            string? answer = Console.ReadLine();
            Console.Clear();
            switch (answer)
            {
                case "1":
                    _bookingConsoleController.SearchBookings();
                    break;
                case "2":
                    _flightConsoleController.RequestFlightsFileName();
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
}
