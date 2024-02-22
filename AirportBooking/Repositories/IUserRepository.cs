using AirportBooking.Models;

namespace AirportBooking.Repositories;

public interface IUserRepository
{
    User? Find(string username);
    User? Find(string username, string password);
    User? Create(User user);
}
