using AirportBooking.Models;

namespace AirportBooking.Repositories;

public interface IUserRepository : IFileRepository<string, User>
{
    User Login(string username, string password);
}
