using AirportBooking.Enums;

namespace AirportBooking.Models;

public class User
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public string ToCSV() => string.Join(",", [Username, Password, Role.ToString()]);

    public override string ToString() => Username;
}
