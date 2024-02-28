using AirportBooking.Exceptions;
using AirportBooking.FileReaders;
using AirportBooking.Models.Users;
using AirportBooking.Serializers.Csv.Users;

namespace AirportBooking.Repositories.Users;

public class UserCsvRepository : IUserCsvRepository
{
    private readonly ICsvFileReader _reader;
    private readonly IUserCsvSerializer _serializer;
    private readonly static string UsersFile = Path.Combine("..", "..", "..", "Data", "users.csv");

    public UserCsvRepository(ICsvFileReader reader, IUserCsvSerializer serializer)
    {
        _reader = reader;
        _serializer = serializer;
    }

    public User? Find(string username)
    {
        return _reader.Read(UsersFile)
            .Select(_serializer.From)
            .Where(u => u.Username == username)
            .FirstOrDefault();
    }

    public User? Find(string username, string password)
    {
        return _reader.Read(UsersFile)
            .Select(_serializer.From)
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
        _reader.Write(UsersFile, _serializer.To(user));
        return user;
    }
}
