using AirportBooking.Constants;
using AirportBooking.Exceptions;
using AirportBooking.FileReaders;
using AirportBooking.Models;
using AirportBooking.Serializers.Csv;

namespace AirportBooking.Repositories;

public class UserRepository : IUserRepository
{
    private List<User> _users = [];
    private readonly CsvFileReader _csvReader = new();
    private readonly UserCsvSerializer _serializer = new();
    private readonly static string fileName = DataDirectory.GetUsersPath();

    public UserRepository()
    {
        try
        {
            LoadUsers();
        }
        catch (Exception ex) when (ex is SerializationException)
        {
            _users.Clear();
            Console.WriteLine("Couldn't load users");
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    private void LoadUsers()
    {
        var readUsers = _csvReader.Read(fileName).ToList();
        readUsers.ForEach(line => _users.Add(_serializer.FromCsv(line)));
    }

    public IReadOnlyCollection<User> FindAll()
    {
        return _users;
    }

    public User Find(string username)
    {
        return _users.Find(u => u.Username.Equals(username))
            ?? throw new EntityNotFound<User, string>(username);
    }

    public User Find(string username, string password)
    {
        return _users.Find(u => u.Username.Equals(username) && u.Password.Equals(password))
            ?? throw new EntityNotFound<User, string>(username);
    }

    public User Create(User user)
    {
        var existingUser = _users.Find(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase));
        if (existingUser is not null)
        {
            throw new EntityAlreadyExists<User, string>(user.Username);
        }
        _csvReader.Write(fileName, _serializer.ToCsv(user));
        _users.Add(user);
        return user;
    }

    public User Update(string username, User user)
    {
        var existingUser = _users.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
            ?? throw new EntityNotFound<User, string>(username); ;
        user.Role = existingUser.Role;
        _csvReader.UpdateLine(fileName, username, _serializer.ToCsv(user));
        _users = _users.Select(u => u.Username.Equals(username) ? user : u).ToList();
        return user;
    }

    public void Delete(string username)
    {
        var existingUser = _users.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
            ?? throw new EntityNotFound<User, string>(username);
        _csvReader.DeleteLine(fileName, username);
        _users.Remove(existingUser);
    }
}
