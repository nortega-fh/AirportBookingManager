using AirportBooking.Exceptions;
using AirportBooking.Models;
using AirportBooking.Repositories;
using AirportBooking.Serializers;

namespace AirportBooking.Controllers;

public class UserController : IController<string, User>
{
    private readonly IUserRepository _userRepository;
    private readonly IConsoleSerializer<User> _serializer;

    public UserController(IUserRepository userRepository, IConsoleSerializer<User> serializer)
    {
        _userRepository = userRepository;
        _serializer = serializer;
    }

    public IReadOnlyList<User> FindAll()
    {
        return _userRepository.FindAll();
    }

    public User? Find(string username, string password)
    {
        return _userRepository.Login(username, password);
    }

    public User? Find(string username)
    {
        return _userRepository.Find(username);
    }

    public User? Create(User user)
    {
        try
        {
            var createdUser = _userRepository.Save(user);
            Console.WriteLine("User succesfully created");
            return createdUser;
        }
        catch (Exception ex) when (ex is EntityAlreadyExists<User, string>)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }

    public User? Update(string username, User newUser)
    {
        try
        {
            return _userRepository.Update(username, newUser);
        }
        catch (Exception ex) when (ex is EntityNotFound<User, string>)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }

    public void Delete(string username)
    {
        _userRepository.Delete(username);
    }

    void IController<string, User>.FindAll()
    {
        throw new NotImplementedException();
    }

    public void Find()
    {
        throw new NotImplementedException();
    }

    void IController<string, User>.Create(User entity)
    {
        throw new NotImplementedException();
    }

    void IController<string, User>.Update(string key, User entity)
    {
        throw new NotImplementedException();
    }
}
