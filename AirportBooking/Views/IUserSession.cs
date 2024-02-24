using AirportBooking.Models;

namespace AirportBooking.Views;

public interface IUserSession
{
    User? GetUser();
    bool IsLoggedIn();
    void SetUser(User user);
    public void LogOutUser();
}