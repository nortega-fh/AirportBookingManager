using AirportBooking.Controllers;
using AirportBooking.Enums;
using AirportBooking.Exceptions;
using AirportBooking.Globals;
using AirportBooking.Models;
using AirportBooking.Views.Menus;

namespace AirportBooking.Views;

public class MainView : BaseConsoleView
{
    private readonly UserRole[] _userRoles = [UserRole.Manager, UserRole.Passenger];
    private readonly UserController _userController;
    private readonly IMenu _managerMenu;
    private readonly IMenu _passengerMenu;

    public MainView(UserController userController, IMenu passengerMenu, IMenu managerMenu)
    {
        _userController = userController;
        _managerMenu = managerMenu;
        _passengerMenu = passengerMenu;
    }

    public void Login()
    {
        Console.WriteLine("Login user");
        var (username, password) = RequestUserData();
        try
        {
            UserSession.SetLoggedUser(_userController.Find(username, password));
            Console.WriteLine("Logged in succesfully. Press enter to continue");
        }
        catch (Exception ex) when (ex is EntityNotFound<User, string>)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            ClearOnInput();
        }
    }

    public void Register()
    {
        Console.WriteLine("Register new User");
        var (username, password) = RequestUserData();
        try
        {
            var createdUser = _userController.Create(
                new User { Username = username, Password = password, Role = UserRole.Passenger });
            UserSession.SetLoggedUser(createdUser);
        }
        catch (Exception ex) when (ex is InvalidAttributeException)
        {
            Console.WriteLine($"Couldn't create User: {ex.Message}");
        }
        finally
        {
            ClearOnInput();
        }
    }

    static (string, string) RequestUserData()
    {
        var username = GetValue("Please type the username");
        var password = GetValue("Please type the password");
        return (username, password);
    }

    public void Show()
    {
        bool isRunning = true;
        const string login = "Login";
        const string register = "Register";
        const string exit = "Exit";
        while (isRunning)
        {
            string option = GetValue("Welcome to the Airport Booking Management System", [login, register, exit]);
            switch (option)
            {
                case login:
                    Login();
                    break;
                case register:
                    Register();
                    break;
                case exit:
                    isRunning = false;
                    break;
            }
            var user = UserSession.GetLoggedUser();
            if (user is null) continue;
            switch (user.Role)
            {
                case UserRole.Manager:
                    _managerMenu.ShowMenu();
                    break;
                default:
                    _passengerMenu.ShowMenu();
                    break;
            }
        }
    }
}