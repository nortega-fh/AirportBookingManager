namespace AirportBooking.Views;

public class PassengerMenu : IPassengerMenu
{
    private readonly IFlightConsoleController _flightConsoleController;
    private readonly IUserSession _userSession;

    public PassengerMenu(IUserSession userSession, IFlightConsoleController flightConsoleController)
    {
        _userSession = userSession;
        _flightConsoleController = flightConsoleController;
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
        throw new NotImplementedException();
    }

    private void ShowBookingManager()
    {
        while (true)
        {
            Console.WriteLine("""
                Booking manager
                1. Show bookings
                2. Edit booking
                3. Delete booking
                4. Go back
                """);
        }
    }
}
