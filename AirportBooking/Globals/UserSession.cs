using AirportBooking.Enums;
using AirportBooking.Models;

namespace AirportBooking.Globals;

public static class UserSession
{
    private static User? _user;

    public static User? GetLoggedUser()
    {
        return _user;
    }

    public static void SetLoggedUser(User? user)
    {
        if (UserContainsInformation()) return;
        if (user is null && _user is not null) return;
        _user = user;
    }

    static bool UserContainsInformation()
    {
        return _user is not null
            && _user.Username is not null
            && !_user.Username.Equals(string.Empty)
            && _user.Password is not null
            && !_user.Password.Equals(string.Empty)
            && _user.Role is UserRole.Passenger or UserRole.Manager;
    }

    public static User? LogOutUser()
    {
        _user = null;
        return _user;
    }
}
