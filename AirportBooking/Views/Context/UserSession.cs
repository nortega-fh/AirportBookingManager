using AirportBooking.Models.Users;

namespace AirportBooking.Views.Context;

public class UserSession : IUserSession
{
    private User? _user;

    public User? GetUser()
    {
        return _user;
    }

    public void SetUser(User user)
    {
        if (IsLoggedIn())
        {
            return;
        }
        _user = user;
    }

    public void LogOutUser()
    {
        _user = null;
    }

    public bool IsLoggedIn()
    {
        return _user is not null;
    }
}
