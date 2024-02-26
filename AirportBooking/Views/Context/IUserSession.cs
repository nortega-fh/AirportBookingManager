using AirportBooking.Models.Users;

namespace AirportBooking.Views.Context;

public interface IUserSession
{
    User? GetUser();
    bool IsLoggedIn();
    void SetUser(User user);
    public void LogOutUser();
}