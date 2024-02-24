namespace AirportBooking.Views;

public class ManagerMenu : IManagerMenu
{
    private readonly IFlightConsoleController _flightConsoleController;
    private readonly IUserSession _userSession;

    public ManagerMenu(IUserSession userSession, IFlightConsoleController flightConsoleController)
    {
        _userSession = userSession;
        _flightConsoleController = flightConsoleController;
    }

    public void ShowManagerMenu()
    {
        while (_userSession.IsLoggedIn())
        {
            Console.WriteLine("""
                Manager Menu
                1. Batch Flight Load
                2. Logout.
                """);
            string? answer = Console.ReadLine();
            Console.Clear();
            switch (answer)
            {
                case "1":
                    _flightConsoleController.RequestFlightsFileName();
                    break;
                case "2":
                    _userSession.LogOutUser();
                    return;
                default:
                    Console.WriteLine("Invalid input, please try again");
                    break;
            }
        }
    }
}
