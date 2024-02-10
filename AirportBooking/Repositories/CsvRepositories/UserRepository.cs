using AirportBooking.Exceptions;
using AirportBooking.FileReaders;
using AirportBooking.Models;
using AirportBooking.Serializers;
using AirportBooking.Validators;
using System.Collections.Immutable;

namespace AirportBooking.Repositories.CsvRepositories;

public class UserRepository : IUserRepository
{
    private List<User> _users = [];
    private readonly CSVReader _csvReader;
    private readonly ICSVSerializer<User> _serializer;
    private readonly IValidator<User> _validator;

    public UserRepository(ICSVSerializer<User> csvSerializer, IValidator<User> validator)
    {
        _validator = validator;
        _serializer = csvSerializer;
        _csvReader = new("users");
        try
        {
            LoadUsers();
        }
        catch (Exception ex) when (ex is InvalidAttributeException or EntitySerializationException<User>)
        {
            _users.Clear();
            Console.WriteLine("Couldn't load users");
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    private void LoadUsers()
    {
        var readUsers = _csvReader.ReadEntityInformation().ToList();
        readUsers.ForEach(line => _users.Add(_serializer.FromCsv(line)));
    }

    public IReadOnlyList<User> FindAll()
    {
        return _users.ToImmutableList();
    }

    public User Find(string username)
    {
        return _users.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
            ?? throw new EntityNotFound<User, string>(username);
    }

    public User Login(string username, string password)
    {
        var user = _users.Find(user => user.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
        && user.Password.Equals(password, StringComparison.OrdinalIgnoreCase));
        return user ?? throw new EntityNotFound<User, string>(username);
    }

    public User Save(User user)
    {
        var existingUser = _users.Find(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase));
        if (existingUser is not null)
        {
            throw new EntityAlreadyExists<User, string>(user.Username);
        }
        _validator.Validate(user);
        _csvReader.WriteEntityInformation(_serializer.ToCsv(user));
        _users.Add(user);
        return user;
    }

    public User Update(string username, User user)
    {
        var existingUser = _users.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
            ?? throw new EntityNotFound<User, string>(username); ;
        user.Role = existingUser.Role;
        _validator.Validate(user);
        _csvReader.UpdateEntityInformation(username, _serializer.ToCsv(user));
        _users = _users.Select(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) ? user : u).ToList();
        return user;
    }

    public void Delete(string username)
    {
        var existingUser = _users.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
            ?? throw new EntityNotFound<User, string>(username);
        _csvReader.DeleteEntityInformation(username);
        _users.Remove(existingUser);
    }
}
