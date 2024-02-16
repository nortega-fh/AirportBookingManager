using AirportBooking.Exceptions;
using AirportBooking.Models;
using AirportBooking.Repositories;
using AirportBooking.Validators.EntityValidators;

namespace AirportBooking.Controllers;

public class UserController
{
    private readonly IUserRepository _userRepository;
    private readonly static UserValidator _validator = new();

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public User? FindUser(string username, string password)
    {
        try
        {
            return _userRepository.Find(username, password);
        }
        catch (EntityNotFound<User, string> ex)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }

    public User? FindUser(string username)
    {
        try
        {
            return _userRepository.Find(username);
        }
        catch (EntityNotFound<User, string> ex)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }

    public User? CreatePassenger(string username, string password)
    {
        try
        {
            var validatedUser = _validator.Validate(new User
            {
                Username = username,
                Password = password,
                Role = Enums.UserRole.Passenger
            });
            return _userRepository.Create(validatedUser);
        }
        catch (EntityAlreadyExists<User, string> ex)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }
}
