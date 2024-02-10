using AirportBooking.Enums;
using AirportBooking.Exceptions;
using AirportBooking.Models;
using AirportBooking.Views.Controllers;

namespace AirportBooking.Views;

public class MainView : ConsoleViewBase
{
    private readonly UserRole[] _userRoles = [UserRole.Manager, UserRole.Passenger];
    private readonly UserController _userController;
    private readonly ManagerView _managerView;
    private readonly PassengerView _passengerView;

    public MainView(UserController userController, PassengerView passengerView, ManagerView managerView)
    {
        _userController = userController;
        _managerView = managerView;
        _passengerView = passengerView;
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
        while (isRunning)
        {
            string option = GetValue("Welcome to the Airport Booking Management System", ["Login", "Register", "Exit"]);
            switch (option)
            {
                case "Login":
                    Login();
                    break;
                case "Register":
                    Register();
                    break;
                case "Exit":
                    isRunning = false;
                    break;
            }
            Console.Clear();
            if (UserSession.GetLoggedUser() is not null)
            {
                ShowMenuForUser();
            }
        }
    }

    void ShowMenuForUser()
    {
        if (UserSession.GetLoggedUser()!.Role is UserRole.Manager)
        {
            _managerView.ShowMenu();
        }
        else
        {
            _passengerView.ShowMenu();
        }
    }
}
