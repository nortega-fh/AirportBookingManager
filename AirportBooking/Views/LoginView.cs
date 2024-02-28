using AirportBooking.ConsoleInputHandler;
using AirportBooking.Models.Users;
using AirportBooking.Repositories.Users;
using AirportBooking.Views.Context;
using AirportBooking.Views.Menus.Manager;
using AirportBooking.Views.Menus.Passenger;

namespace AirportBooking.Views;

public class LoginView
{
    private readonly IUserCsvRepository _userRepository;
    private readonly IUserSession _userSession;
    private readonly IManagerMenu _managerMenu;
    private readonly IPassengerMenu _passengerMenu;
    private readonly IConsoleInputHandler _consoleInputHandler;

    public LoginView(
        IUserCsvRepository userRepository,
        IUserSession userSession,
        IManagerMenu managerMenu,
        IPassengerMenu passengerMenu,
        IConsoleInputHandler consoleInputHandler)
    {
        _userRepository = userRepository;
        _managerMenu = managerMenu;
        _passengerMenu = passengerMenu;
        _userSession = userSession;
        _consoleInputHandler = consoleInputHandler;
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
            Console.Clear();
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
            if (_userSession.IsLoggedIn())
            {
                Console.Clear();
                if (_userSession.GetUser()!.Role is UserRole.Manager)
                {
                    _managerMenu.ShowManagerMenu();
                }
                else
                {
                    _passengerMenu.ShowPassengerMenu();
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
        _userSession.SetUser(user);
    }

    private void Register()
    {
        var username = _consoleInputHandler.GetNotEmptyString("username");
        var password = _consoleInputHandler.GetNotEmptyString("password");
        try
        {
            _userSession.SetUser(_userRepository.Create(
                new User
                {
                    Username = username,
                    Password = password,
                    Role = UserRole.Passenger
                }));
            Console.WriteLine("User registered succesfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error registering user:");
            Console.WriteLine(ex.Message);
        }
    }
}