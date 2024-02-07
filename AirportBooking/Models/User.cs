using AirportBooking.Enums;

namespace AirportBooking.Models;

public class User
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public override string ToString() => Username;
}
