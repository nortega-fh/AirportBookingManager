using AirportBooking.Enums;
using AirportBooking.Exceptions;
using AirportBooking.Models;
using AirportBooking.Repositories;
using AirportBooking.Serializers;

namespace AirportBooking.Views.ModelViews;

public class UserView : ViewInputHandler, IUserView
{
    private readonly IUserRepository _userRepository;
    private readonly IConsoleSerializer<User> _serializer;
    private const string requestUsernameMessage = "Please type the User's username";
    private const string requestPasswordMessage = "Please type the User's password";

    public UserView(IUserRepository userRepository, IConsoleSerializer<User> serializer)
    {
        _userRepository = userRepository;
        _serializer = serializer;
    }

    public void ShowAll()
    {
        foreach (var user in _userRepository.FindAll())
        {
            _serializer.PrintToConsole(user);
        }
    }

    public void ShowOne()
    {
        var username = GetValue(requestUsernameMessage);
        try
        {
            _serializer.PrintToConsole(_userRepository.Find(username));
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

    public void Create()
    {
        CreateUser(UserRole.Passenger);
    }

    public void Create(UserRole role)
    {
        CreateUser(role);
    }

    public User? Register()
    {
        Console.WriteLine("Register new User");
        return CreateUser(UserRole.Passenger);
    }

    private User? CreateUser(UserRole role)
    {
        var username = GetValue(requestUsernameMessage);
        var password = GetValue(requestPasswordMessage);
        try
        {
            var user = _userRepository.Save(new User { Username = username, Password = password, Role = role });
            Console.WriteLine("User succesfully created");
            return user;
        }
        catch (Exception ex) when (ex is InvalidAttributeException or EntityAlreadyExists<User, string>)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            ClearOnInput();
        }
        return null;
    }

    public void Update()
    {
        var username = GetValue(requestUsernameMessage);
        var password = GetValue(requestPasswordMessage);
        try
        {
            _userRepository.Update(username, new User { Username = username, Password = password });
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

    public void Delete()
    {
        try
        {
            _userRepository.Delete(GetValue(requestUsernameMessage));
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

    public User? Login()
    {
        Console.WriteLine("Login user");
        var username = GetValue(requestUsernameMessage);
        var password = GetValue(requestPasswordMessage);
        User? foundUser = null;
        try
        {
            foundUser = _userRepository.Login(username, password);
        }
        catch (Exception ex) when (ex is EntityNotFound<User, string>)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            ClearOnInput();
        }
        return foundUser;
    }
}
