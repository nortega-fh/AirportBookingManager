using AirportBooking.Models.Users;

namespace AirportBooking.Repositories.Users;

public interface IUserCsvRepository
{
    User? Find(string username);
    User? Find(string username, string password);
    User Create(User user);
}
