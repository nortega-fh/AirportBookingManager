using AirportBooking.Enums;
using AirportBooking.Models;
using AirportBooking.Repositories;

namespace AirportBooking.Views;

public class MainView
{
    private readonly IUserRepository _userRepository;

    public MainView(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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
    }

    private void Register()
    {
        string username = "";
        while (username is "")
        {
            Console.WriteLine("Please type your username");
            username = Console.ReadLine() ?? "";
            if (username is "") Console.WriteLine("You have to type a valid username");
        }
        var password = GetNotEmptyString("password");

        try
        {
            _userRepository.Create(new User { Username = username, Password = password, Role = UserRole.Passenger });
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
}