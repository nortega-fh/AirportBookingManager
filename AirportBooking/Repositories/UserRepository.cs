using AirportBooking.Exceptions;
using AirportBooking.FileReaders;
using AirportBooking.Models;
using AirportBooking.Serializers.Csv;

namespace AirportBooking.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IFileReader _reader;
    private readonly UserCsvSerializer _serializer = new();
    private readonly static string UsersFile = Path.Combine("..", "..", "..", "Data", "users.csv");

    public UserRepository(IFileReader reader)
    {
        _reader = reader;
    }

    public User? Find(string username)
    {
        return _reader.Read(UsersFile)
            .Select(_serializer.FromCsv)
            .Where(u => u.Username == username)
            .FirstOrDefault();
    }

    public User? Find(string username, string password)
    {
        return _reader.Read(UsersFile)
            .Select(_serializer.FromCsv)
            .Where(u => u.Username.Equals(username) && u.Password.Equals(password))
            .FirstOrDefault();
    }

    public User Create(User user)
    {
        var existingUser = Find(user.Username);
        if (existingUser is not null)
        {
            throw new EntityAlreadyExists<User, string>(user.Username);
        }
        _reader.Write(UsersFile, _serializer.ToCsv(user));
        return user;
    }
}
